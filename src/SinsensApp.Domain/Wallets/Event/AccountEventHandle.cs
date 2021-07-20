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
    public class AccountEventHandle : DomainService, ITransientDependency,
        ILocalEventHandler<EntityUpdatingEventData<Account>>,
        ILocalEventHandler<EntityCreatedEventData<Transaction>>,
        ILocalEventHandler<EntityUpdatedEventData<Transaction>>,
        ILocalEventHandler<EntityDeletedEventData<Transaction>>
    {
        private readonly IClock _clock;
        private readonly IRepository<Account, Guid> _accounts;
        private readonly ITransactionRepository _transactions;

        public AccountEventHandle(
            IClock clock,
            IRepository<Account, Guid> accounts,
            ITransactionRepository transactions
            )
        {
            _clock = clock;
            _accounts = accounts;
            _transactions = transactions;
        }

        /// <summary>
        /// 账户初始化
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityCreatedEventData<Account> eventData)
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
            };
            if (account.Balance < 0)
            {
                transaction.AccountFrom = account;
                transaction.TransactionType = TransactionType.Expenditure;
            }
            else
            {
                transaction.AccountTo = account;
                transaction.TransactionType = TransactionType.Income;
            }
            await _transactions.InsertAsync(transaction);
        }

        /// <summary>
        /// 账户变更，更新账户时生成一条对应的交易记录，多退少补
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityUpdatingEventData<Account> eventData)
        {
            var account = eventData.Entity;
            var oldBalance = _transactions.Where(x => x.AccountToId == account.Id).Sum(x => x.Amount);
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

        /// <summary>
        /// 交易创建,更新账户余额
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task HandleEventAsync(EntityCreatedEventData<Transaction> eventData)
        {
            await UpdateAccountBalance(eventData.Entity);
        }

        /// <summary>
        /// 交易变更,更新账户余额
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task HandleEventAsync(EntityUpdatedEventData<Transaction> eventData)
        {
            await UpdateAccountBalance(eventData.Entity);
        }

        /// <summary>
        /// 交易删除,更新账户余额
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task HandleEventAsync(EntityDeletedEventData<Transaction> eventData)
        {
            await UpdateAccountBalance(eventData.Entity);
        }

        [UnitOfWork]
        private async Task UpdateAccountBalance(Transaction transaction)
        {
            Account accountFrom = _accounts.Where(x => x.Id == transaction.AccountFromId).FirstOrDefault();
            Account accountTo = _accounts.Where(x => x.Id == transaction.AccountToId).FirstOrDefault();

            // todo 汇率计算
            if (accountFrom != null)
            {
                var income = _transactions.Where(x => x.AccountToId == accountFrom.Id).Sum(x => x.Amount);
                var expenditure = _transactions.Where(x => x.AccountFromId == accountFrom.Id).Sum(x => x.Amount);
                accountFrom.Balance = income - expenditure;
                await _accounts.UpdateAsync(accountFrom);
            }
            if (accountTo != null)
            {
                var income = _transactions.Where(x => x.AccountToId == accountTo.Id).Sum(x => x.Amount);
                var expenditure = _transactions.Where(x => x.AccountFromId == accountTo.Id).Sum(x => x.Amount);
                accountTo.Balance = income - expenditure;
                await _accounts.UpdateAsync(accountTo);
            }
        }
    }
}