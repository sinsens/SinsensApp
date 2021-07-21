using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace SinsensApp.Wallets.Event
{
    public class AccountEventHandle : ITransientDependency,
        ILocalEventHandler<AccountCreatedEventEto>,
        ILocalEventHandler<AccountUpdatingEventEto>
    {
        private readonly IClock _clock;
        private readonly ITransactionRepository _transactions;

        public AccountEventHandle(
            IClock clock,
            ITransactionRepository transactions,
            IRateRepository rates
            )
        {
            _clock = clock;
            _transactions = transactions;
        }

        public async Task HandleEventAsync(AccountUpdatingEventEto eventData)
        {
            var account = eventData.Entity;
            var oldBalance = await _transactions.Where(x => x.AccountToId == account.Id).SumAsync(x => x.Amount);
            // 应有余额，多退少补
            if (account.Balance != oldBalance)
            {
                var transaction = new Transaction()
                {
                    TenantId = account.TenantId,
                    Note = "账户变更",
                    UserId = account.UserId,
                    Date = _clock.Now
                };
                if (account.Balance < oldBalance)
                {
                    transaction.AccountFromId = account.Id;
                    transaction.TransactionType = TransactionType.Expenditure;
                    transaction.Amount = oldBalance - account.Balance;
                }
                else
                {
                    transaction.AccountToId = account.Id;
                    transaction.TransactionType = TransactionType.Income;
                    transaction.Amount = account.Balance - oldBalance;
                }
                await _transactions.InsertAsync(transaction);
            }
        }

        public async Task HandleEventAsync(AccountCreatedEventEto eventData)
        {
            // 初始化账户时生成一条对应的交易记录
            var account = eventData.Entity;
            var transaction = new Transaction()
            {
                TenantId = account.TenantId,
                UserId = account.UserId,
                Note = "账户初始化",
                Date = _clock.Now,
                Amount = account.Balance,
                TransactionState = true
            };
            if (account.Balance < 0)
            {
                transaction.AccountFrom = account;
                transaction.AccountFromId = account.Id;
                transaction.TransactionType = TransactionType.Expenditure;
            }
            else
            {
                transaction.AccountTo = account;
                transaction.AccountToId = account.Id;
                transaction.TransactionType = TransactionType.Income;
            }
            await _transactions.InsertAsync(transaction);
        }
    }
}