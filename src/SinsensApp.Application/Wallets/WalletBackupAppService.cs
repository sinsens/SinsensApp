using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinsensApp.Wallets.Dtos.backup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Volo.Abp.ObjectMapping;

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

        public WalletBackupAppService(
            IWebHostEnvironment webHostEnvironment,
            IJsonSerializer jsonSerializer,
            IAccountRepository accounts,
             ICategoryRepository categories,
              ITagRepository tags,
              ITransactionRepository transactions,
              IRepository<Currency, string> currencies,
              IHttpContextAccessor httpContextAccessor
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
        }

        [HttpGet]
        public async Task<WalletBackupRequestResultDto> Backup()
        {
            var result = new WalletBackupRequestResultDto();

            var accounts = await _repositoryAccount.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id, false);
            if (!accounts.Any())
            {
                throw new UserFriendlyException("没有可用账户信息");
            }
            var currencies = await _repositoryCurrency.GetListAsync(false);
            var categories = await _repositoryCategory.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id, false);
            var tags = await _repositoryTag.GetListAsync();
            var transactions = await _repositoryTransaction.IncludeDetails(true).Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id).ToListAsync();

            var backupJson = new BackupJsonDto();

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

            var filename = $"WalletBackupJson_{Clock.Now.ToString("yyyy-MM-dd")}_{GuidGenerator.Create().ToString("n").Substring(20, 5)}.json";
            var tempSaveDir = Path.Combine(_hostingEnvironment.WebRootPath, $"Temp{Path.DirectorySeparatorChar}Downloads{Path.DirectorySeparatorChar}");
            var tempSavePath = $"{tempSaveDir}{filename}";
            var jsonContent = _jsonSerializer.Serialize(backupJson);

            if (Directory.Exists(tempSaveDir) == false)
            {
                Directory.CreateDirectory(tempSaveDir);
            }
            await File.WriteAllTextAsync(tempSavePath, jsonContent);

            result.Url = $"/Temp/Downloads/{filename}";
            result.Size = new FileInfo(tempSavePath).Length;

            return result;
        }

        [HttpPost]
        public async Task<string> Restore(WalletRestoreRequestDto walletRestoreRequest)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var postFile = request.Form.Files;
            if (postFile == null)
            {
                throw new UserFriendlyException("请上传备份文件");
            }

            BackupJsonDto backupJson = null;
            using (var stream = postFile[0].OpenReadStream())
            using (var reader = new StringWriter())
            {
                var bytes = await stream.GetAllBytesAsync();
                reader.Write(bytes);
                backupJson = _jsonSerializer.Deserialize<BackupJsonDto>(reader.ToString());
            }
            var accounts = await _repositoryAccount.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id, false);
            var currencies = await _repositoryCurrency.GetListAsync(false);
            var categories = await _repositoryCategory.GetListAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id, false);
            var tags = await _repositoryTag.GetListAsync();
            var transactions = await _repositoryTransaction.IncludeDetails(true).Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id).ToListAsync();

            return "导入成功";
        }
    }
}