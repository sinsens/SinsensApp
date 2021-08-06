using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
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
    public class ExpenseForcastModelBuilderEventHandle : IDistributedEventHandler<StartExpenseForcastModelBuilderEto>, ITransientDependency
    {
        private readonly ITransactionRepository _repository;
        private readonly IDistributedEventBus _eventBus;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public ExpenseForcastModelBuilderEventHandle(
            ITransactionRepository repository,
            IDistributedEventBus eventBus,
            IBackgroundJobManager backgroundJobManager)
        {
            _repository = repository;
            _eventBus = eventBus;
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task Execute(StartExpenseForcastModelBuilderEto args)
        {
            var tenantId = args.TenantId.HasValue ? args.TenantId.Value.ToString("n") : "Default";
            var userId = args.UserId.HasValue ? args.UserId.Value.ToString("n") : "Default";
            var transactions = await _repository.Where(x => x.TransactionType == TransactionType.Expenditure && (x.UserId == args.UserId || x.CreatorId == args.TenantId)).OrderBy(x => x.Date).ToListAsync();
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
                // 写出训练数据
                await File.WriteAllLinesAsync(filePath, content);

                // 后台训练
                await _backgroundJobManager.EnqueueAsync(new ExpenseForcastModelBuilderInputDto { TenantID = args.TenantId, UserID = args.UserId, DataFilePath = filePath });
            }
        }

        public async Task HandleEventAsync(StartExpenseForcastModelBuilderEto eventData)
        {
            await Task.Run(async () => { await Execute(eventData); });
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