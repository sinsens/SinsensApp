using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SinsensApp.Wallets.Dtos.backup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace SinsensApp.Wallets
{
    [Authorize]
    public class WalletBackupAppService : SinsensAppAppService, IWalletBackupService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IAccountRepository _repositoryAccount;
        private readonly ICategoryRepository _repositoryCategory;
        private readonly ITagRepository _repositoryTag;
        private readonly ITransactionRepository _repositoryTransaction;
        private readonly IRepository<Currency, string> _repositoryCurrency;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataFilter _dataFilter;
        private readonly IDistributedCache<WalletBackupRequestResultDto, string> _cache;

        public WalletBackupAppService(
            IWebHostEnvironment webHostEnvironment,
            IJsonSerializer jsonSerializer,
            IAccountRepository accounts,
            ICategoryRepository categories,
            ITagRepository tags,
            ITransactionRepository transactions,
            IRepository<Currency, string> currencies,
            IHttpContextAccessor httpContextAccessor,
            IDataFilter dataFilter,
            IDistributedCache<WalletBackupRequestResultDto, string> cache
        )
        {
            _hostingEnvironment = webHostEnvironment;
            _jsonSerializer = jsonSerializer;
            _repositoryAccount = accounts;
            _repositoryCategory = categories;
            _repositoryTag = tags;
            _repositoryTransaction = transactions;
            _repositoryCurrency = currencies;
            _httpContextAccessor = httpContextAccessor;
            _dataFilter = dataFilter;
            _cache = cache;
        }

        [HttpGet]
        public async Task<WalletBackupRequestResultDto> Backup()
        {
            var accounts = await _repositoryAccount.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id, false);
            if (!accounts.Any())
            {
                throw new UserFriendlyException("没有可用账户信息");
            }
            return await _cache.GetOrAddAsync($"JSON_backup_{CurrentTenant.Name}_{CurrentUser.UserName}", async () =>
            {
                return await BackupToJson();
            },
            () => new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(WalletBackupRequestResultDto.CacheMinute) // 设置 5 分钟内只能备份一次
            });
        }

        [HttpPost]
        [UnitOfWork]
        public async Task<string> Restore(bool clearBeforeRestore = false, bool skipIfExists = true)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var postFile = request.Form.Files.FirstOrDefault();
            if (postFile == null)
            {
                throw new UserFriendlyException("请上传备份文件");
            }

            BackupJsonDto restoreJson = null;
            using (var stream = postFile.OpenReadStream())
            {
                var bytes = await stream.GetAllBytesAsync();
                if (bytes.LongLength > 1024 * 1024 * 10)
                {
                    throw new UserFriendlyException("上传文件大小不得超过 10 MB");
                }
                var content = Encoding.Default.GetString(bytes);
                restoreJson = JsonConvert.DeserializeObject<BackupJsonDto>(content);
                if (restoreJson == null)
                {
                    throw new UserFriendlyException("上传文件格式不正确，反序列化失败");
                }
            }

            // 先进行备份
            var cacheKey = $"JSON_restore_{CurrentTenant.Name}_{CurrentUser.UserName}";
            var backupJson = await _cache.GetAsync(cacheKey);
            if (backupJson == null)
            {
                backupJson = await _cache.GetOrAddAsync(cacheKey, async () =>
                {
                    return await BackupToJson();
                },
                () => new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(WalletBackupRequestResultDto.CacheMinute) // 设置 5 分钟内只能还原一次
                });
            }
            else
            {
                return "操作过于频繁，请 5 分钟后再试";
            }

            try
            {
                using (_dataFilter.Disable<ISoftDelete>())
                using (_dataFilter.Disable<IMultiTenant>())
                {
                    var accounts = await _repositoryAccount.Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id).ToListAsync();
                    var categories = await _repositoryCategory.Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id).ToListAsync();
                    var currencies = await _repositoryCurrency.GetListAsync(false);
                    var tags = await _repositoryTag.Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id).ToListAsync();
                    var transactions = await _repositoryTransaction.IncludeDetails(true).Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id).ToListAsync();

                    var oldAccountIds = accounts.Select(x => x.Id).ToHashSet();
                    var oldCategoryIds = categories.Select(x => x.Id).ToHashSet();
                    var oldTagsIds = tags.Select(x => x.Id).ToHashSet();
                    var oldTransactionIds = transactions.Select(x => x.Id).ToHashSet();

                    // 清理现有数据，硬删除
                    if (clearBeforeRestore)
                    {
                        await _repositoryTransaction.HardDeleteAsync(x => oldTransactionIds.Contains(x.Id));
                        await _repositoryAccount.HardDeleteAsync(x => oldAccountIds.Contains(x.Id));
                        await _repositoryCategory.HardDeleteAsync(x => oldCategoryIds.Contains(x.Id));
                        await _repositoryTag.HardDeleteAsync(x => oldTagsIds.Contains(x.Id));
                    }

                    var accountForInsert = restoreJson.accounts.WhereIf(!clearBeforeRestore, x => !oldAccountIds.Contains(x.id))
                        .Select(x => ObjectMapper.Map<AccountsItemDto, Account>(x)).ToList();
                    var categoryForInsert = restoreJson.categories.WhereIf(!clearBeforeRestore, x => !oldCategoryIds.Contains(x.id))
                        .Select(x => ObjectMapper.Map<CategoriesItemDto, Category>(x)).ToList();
                    var tagForInsert = restoreJson.tags.WhereIf(!clearBeforeRestore, x => !oldTagsIds.Contains(x.id))
                        .Select(x => ObjectMapper.Map<TagsItemDto, Tag>(x)).ToList();
                    var transactionForInsert = restoreJson.transactions.WhereIf(!clearBeforeRestore, x => !oldTransactionIds.Contains(x.id))
                        .GroupBy(x => x.id)
                        .Select(ls => ObjectMapper.Map<TransactionsItemDto, Transaction>(ls.FirstOrDefault())).ToList();

                    // 执行更新
                    if (!clearBeforeRestore && !skipIfExists)
                    {
                        var accountForUpdate = restoreJson.accounts.Where(x => oldAccountIds.Contains(x.id))
                            .Select(x => ObjectMapper.Map<AccountsItemDto, Account>(x)).ToList();
                        var categoryForUpdate = restoreJson.categories.Where(x => oldCategoryIds.Contains(x.id))
                            .Select(x => ObjectMapper.Map<CategoriesItemDto, Category>(x)).ToList();
                        var tagForUpdate = restoreJson.tags.Where(x => oldTagsIds.Contains(x.id))
                            .Select(x => ObjectMapper.Map<TagsItemDto, Tag>(x)).ToList();

                        await _repositoryAccount.UpdateManyAsync(accountForUpdate);
                        await _repositoryCategory.UpdateManyAsync(categoryForUpdate);
                        await _repositoryTag.UpdateManyAsync(tagForUpdate);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        // 处理交易标签
                        foreach (var item in transactionForInsert)
                        {
                            var transaction = restoreJson.transactions.Find(x => x.id == item.Id);
                            item.Tags = await _repositoryTag.Where(x => transaction.tag_ids.Contains(x.Id)).ToListAsync();
                        }
                        var transactionForUpdate = restoreJson.transactions.Where(x => oldTransactionIds.Contains(x.id))
                            .Select(x => ObjectMapper.Map<TransactionsItemDto, Transaction>(x)).ToList();
                        await _repositoryTransaction.UpdateManyAsync(transactionForUpdate);
                    }

                    // 执行插入
                    {
                        foreach (var item in restoreJson.currencies)
                        {
                            if (currencies.Any(x => x.Code == item.code) == false)
                            {
                                var model = ObjectMapper.Map(item, new Currency());
                                await _repositoryCurrency.InsertAsync(model);
                                currencies.Add(model);
                            }
                        }

                        await _repositoryAccount.InsertManyAsync(accountForInsert);
                        await _repositoryCategory.InsertManyAsync(categoryForInsert);
                        await _repositoryTag.InsertManyAsync(tagForInsert);
                        await _repositoryTransaction.InsertManyAsync(transactionForInsert);
                    }
                }
            }
            catch (Exception)
            {
                await _cache.RemoveAsync(cacheKey);
                throw;
            }

            return $"还原成功，原有数据已自动备份到：{backupJson.Url}， 备份大小为：{backupJson.Size / 1024 } KB";
        }

        private async Task<WalletBackupRequestResultDto> BackupToJson()
        {
            var result = new WalletBackupRequestResultDto();
            var backupJson = new BackupJsonDto();

            using (_dataFilter.Disable<ISoftDelete>())
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var accounts = await _repositoryAccount.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id, false);

                var currencies = await _repositoryCurrency.GetListAsync(false);
                var categories = await _repositoryCategory.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id, false);
                var tags = await _repositoryTag.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id);
                var transactions = await _repositoryTransaction.IncludeDetails(true).Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id).ToListAsync();

                foreach (var item in accounts)
                {
                    backupJson.accounts.Add(ObjectMapper.Map<Account, AccountsItemDto>(item));
                }
                foreach (var item in currencies)
                {
                    backupJson.currencies.Add(ObjectMapper.Map<Currency, CurrenciesItemDto>(item));
                }
                foreach (var item in categories)
                {
                    backupJson.categories.Add(ObjectMapper.Map<Category, CategoriesItemDto>(item));
                }
                foreach (var item in tags)
                {
                    backupJson.tags.Add(ObjectMapper.Map<Tag, TagsItemDto>(item));
                }
                foreach (var item in transactions)
                {
                    var transaction = new TransactionsItemDto();
                    ObjectMapper.Map<Transaction, TransactionsItemDto>(item, transaction);
                    backupJson.transactions.Add(transaction);
                }
            }

            var filename = $"WalletBackupJson_{Clock.Now.ToString("yyyy-MM-dd")}_{GuidGenerator.Create().ToString("n").Substring(20, 5)}.json";
            var tempSaveDir = Path.Combine(_hostingEnvironment.WebRootPath, $"Temp{Path.DirectorySeparatorChar}Downloads{Path.DirectorySeparatorChar}{CurrentTenant.Name}{Path.DirectorySeparatorChar}{CurrentUser.UserName}{Path.DirectorySeparatorChar}");
            var tempSavePath = $"{tempSaveDir}{filename}";
            var jsonContent = _jsonSerializer.Serialize(backupJson);

            if (Directory.Exists(tempSaveDir) == false)
            {
                Directory.CreateDirectory(tempSaveDir);
            }
            await File.WriteAllTextAsync(tempSavePath, jsonContent);

            result.Url = $"/Temp/Downloads/{CurrentTenant.Name}/{CurrentUser.UserName}/{filename}";
            result.Size = new FileInfo(tempSavePath).Length;

            return result;
        }
    }
}