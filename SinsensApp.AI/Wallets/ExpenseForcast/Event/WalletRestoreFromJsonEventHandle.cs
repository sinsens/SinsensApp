using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SinsensApp.AI.Event.Eto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace SinsensApp.Wallets.Event
{
    public class WalletRestoreFromJsonEventHandle : IDistributedEventHandler<WalletRestoreFromJsonEto>, ITransientDependency
    {
        private readonly ITransactionRepository _repository;
        private readonly IDistributedEventBus _eventBus;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ILogger<WalletRestoreFromJsonEventHandle> _logger;

        public WalletRestoreFromJsonEventHandle(
            ITransactionRepository repository,
            IDistributedEventBus eventBus,
            IBackgroundJobManager backgroundJobManager,
            ILogger<WalletRestoreFromJsonEventHandle> logger)
        {
            _repository = repository;
            _eventBus = eventBus;
            _backgroundJobManager = backgroundJobManager;
            _logger = logger;
        }

        public async Task Execute(WalletRestoreFromJsonEto args)
        {
            _logger.LogInformation("=============== 导出训练数据 ===============");
            var tenantId = args.TenantId.HasValue ? args.TenantId.Value.ToString("n") : "Default";
            var userId = args.UserId.HasValue ? args.UserId.Value.ToString("n") : "Default";
            var transactions = await _repository.Where(x => x.TransactionType == TransactionType.Expenditure &&
                x.TenantId == args.TenantId &&
                (x.UserId == args.UserId || x.CreatorId == args.UserId)).OrderBy(x => x.Date).ToListAsync();

            if (transactions.Count > 50)
            {
                var datas = new SortedDictionary<string, decimal>();
                foreach (var item in transactions)
                {
                    var day = item.Date.Value.ToString("yyyy-M-d");
                    if (!datas.ContainsKey(day))
                    {
                        datas.Add(day, item.Amount);
                    }
                    else
                    {
                        datas[day] += item.Amount;
                    }
                }
                var filePath = GetAbsolutePath(tenantId, userId);
                var content = datas.Select(x => $"{x.Key},{x.Value}");
                if (content.Count() < 50)
                {
                    _logger.LogInformation("=============== 训练数据不足，至少需要50条记录 ===============");
                    return;
                }
                // 写出训练数据
                await File.WriteAllLinesAsync(filePath, content);

                // 后台训练
                await _backgroundJobManager.EnqueueAsync(new ExpenseForcastModelBuilderInputDto { TenantID = args.TenantId, UserID = args.UserId, DataFilePath = filePath });
            }
            else
            {
                _logger.LogInformation("=============== 训练数据不足");
            }
        }

        public async Task HandleEventAsync(WalletRestoreFromJsonEto eventData)
        {
            await Execute(eventData);
        }

        public static string GetAbsolutePath(string tenantId, string userId)
        {
            var dataPath = $"{AppDomain.CurrentDomain.BaseDirectory}/TrainData/{tenantId}/{userId}_day.txt";
            var fileInfo = new FileInfo(dataPath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            return dataPath;
        }
    }
}