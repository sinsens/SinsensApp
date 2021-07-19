using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace SinsensApp.Wallets
{
    public class AutoSyncCurrencyRates : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Currency, string> _repositoryCurrency;
        private readonly IRepository<Rate, string> _repositoryRate;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly NullLogger _logger;

        public AutoSyncCurrencyRates(AbpTimer timer,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Currency, string> repositoryCurrency,
            IRepository<Rate, string> repositoryRate,
            IJsonSerializer jsonSerializer,
            IServiceScopeFactory serviceScopeFactory
            ) : base(timer, serviceScopeFactory)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _repositoryCurrency = repositoryCurrency;
            _repositoryRate = repositoryRate;
            _jsonSerializer = jsonSerializer;
            _logger = NullLogger.Instance;

            timer.Period = 24 * 3600 * 1000; // 每天执行一次

            Task.Run(() => Sync());
        }

        protected override void DoWork(PeriodicBackgroundWorkerContext workerContext)
        {
            Task.Run(() => Sync());
        }

        protected async virtual Task Sync()
        {
            _logger.LogInformation("开始同步汇率数据");
            var currencies = await _repositoryCurrency.GetListAsync();
            foreach (var currency in currencies)
            {
                _logger.LogInformation("同步货币 {0} 汇率", currency.Code);
                await SyncRate(currency);
                using (var ouw = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                {
                    await _repositoryCurrency.UpdateAsync(currency);
                }
            }
            _logger.LogInformation("同步汇率数据结束");
        }

        protected virtual async Task SyncRate(Currency currency)
        {
            // 初始化货币信息
            var apiUrl = $"{WalletsConsts.DefaultCurrencyRateApiBaseUrl}{currency.Code}";
            var client = new WebClient();
            var result = await client.DownloadStringTaskAsync(apiUrl);
            var ratesInfo = _jsonSerializer.Deserialize<RatesInfo>(result);
            var currencyRate = currency.CurrencyRate;
            if (currencyRate == null)
            {
                currencyRate = new CurrencyRate(currency.Code);
                currencyRate.Code = currency.Code;
            }
            currencyRate.Date = ratesInfo.date;
            currencyRate.Last_Updated = ratesInfo.time_last_updated;
            currencyRate.Provider = ratesInfo.provider;

            currencyRate.Rates = ratesInfo.rates.Select(x => new Rate(currency.Code, x.Key, x.Value)).ToList();
            currency.CurrencyRate = currencyRate;
            await _repositoryRate.DeleteManyAsync(currencyRate.Rates.Select(x => x.Id), true);
            await _repositoryRate.InsertManyAsync(currencyRate.Rates, true);
        }
    }
}