using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SinsensApp.Wallets.Dtos.backup;
using SinsensApp.Wallets.Dtos.statics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Volo.Abp.Timing;

namespace SinsensApp.Wallets
{
    [Authorize]
    public class StatAppService : SinsensAppAppService, IStatAppService
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IAccountRepository _repositoryAccount;
        private readonly ICategoryRepository _repositoryCategory;
        private readonly ITagRepository _repositoryTag;
        private readonly ITransactionRepository _repositoryTransaction;
        private readonly IDistributedCache<PeriodExpenseStatRequestResultDto, string> _cache;

        public StatAppService(IWebHostEnvironment webHostEnvironment,
            IJsonSerializer jsonSerializer,
            IAccountRepository accounts,
            ICategoryRepository categories,
            ITagRepository tags,
            ITransactionRepository transactions,
            IRepository<Currency, string> currencies,
            IDistributedCache<PeriodExpenseStatRequestResultDto, string> cache)
        {
            _jsonSerializer = jsonSerializer;
            _repositoryAccount = accounts;
            _repositoryCategory = categories;
            _repositoryTag = tags;
            _repositoryTransaction = transactions;
            _cache = cache;
        }

        public async Task<PeriodExpenseStatRequestResultDto> GetPeriodResultList(PeriodExpenseStatRequestDto input)
        {
            var cacheKey = $"WALLETS_STAT_result_{CurrentTenant.Id}_{CurrentUser.UserName}_{input.StartTime}_{input.EndTime}";
            return await _cache.GetOrAddAsync(cacheKey,
                async () => { return await GetPeriodResult(input); },
                () =>
                new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = Clock.Now.AddMinutes(PeriodExpenseStatRequestDto.CacheMinute) // 设置 5 分钟内只能备份一次
                }
            );
        }

        private async Task<PeriodExpenseStatRequestResultDto> GetPeriodResult(PeriodExpenseStatRequestDto input)
        {
            var result = new PeriodExpenseStatRequestResultDto();
            if (await _repositoryAccount.AnyAsync(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id) == false)
            {
                return result;
            }

            input.EndTime = input.EndTime.HasValue ? input.EndTime.Value.AddSeconds(1) : (DateTime?)null;

            var query = await _repositoryTransaction.GetQueryableAsync();
            query = query.AsNoTracking().IncludeDetails()
                .Where(x => x.IncludeInReports)
                .Where(x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id)
                .WhereIf(input.StartTime.HasValue, x => x.Date >= input.StartTime)
                .WhereIf(input.EndTime.HasValue, x => x.Date <= input.EndTime.Value);

            var transactions = await query.ToListAsync();
            var count = transactions.Count;
            for (int i = 0; i < count; i++)
            {
                var transaction = transactions[i];
                switch (transaction.TransactionType)
                {
                    case TransactionType.Expenditure:
                        result.Expenditure += transaction.Amount;
                        break;

                    case TransactionType.Income:
                        result.Income += transaction.Amount;
                        break;

                    case TransactionType.Transfer:
                        result.Transfer += transaction.Amount;
                        break;
                }
                result.Total += transaction.Amount;

                if (transaction.Category != null && transaction.TransactionType == TransactionType.Expenditure)
                {
                    var category = result.Items.Where(x => x.Text == transaction.Category.Title).FirstOrDefault();
                    if (category == null)
                    {
                        category = new PeriodResultListItemDto();
                        ObjectMapper.Map(transaction.Category, category);
                        result.Items.Add(category);
                    }
                    category.Value += transaction.Amount;

                    if (transaction.Tags.Any())
                    {
                        foreach (var item in transaction.Tags)
                        {
                            var tag = category.Children.Where(x => x.Text == item.Title).FirstOrDefault();
                            if (tag == null)
                            {
                                tag = new PeriodResultListItemDto();
                                ObjectMapper.Map(item, tag);
                                category.Children.Add(tag);
                            }
                            tag.Value += transaction.Amount;
                        }
                    }
                }
            }

            if (result.Expenditure > 0)
                foreach (var item in result.Items)
                {
                    item.Percent = (float)Math.Round(item.Value / result.Expenditure, 4);
                }
            return result;
        }
    }
}