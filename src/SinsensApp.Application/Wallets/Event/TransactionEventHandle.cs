using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using static SinsensApp.Permissions.SinsensAppPermissions;
using Volo.Abp.Uow;
using Volo.Abp.Timing;

namespace SinsensApp.Wallets.Event
{
    public class TransactionEventHandle : ILocalEventHandler<TransactionCreatingEventEto>, ILocalEventHandler<TransactionUpdatedEventEto>, ITransientDependency
    {
        private readonly IClock _clock;
        private readonly IAccountRepository _accounts;
        private readonly ITransactionRepository _transactions;

        public TransactionEventHandle(
            IClock clock,
            IAccountRepository accounts,
            ITransactionRepository transactions,
            IRateRepository rates
            )
        {
            _clock = clock;
            _accounts = accounts;
            _transactions = transactions;
        }

        public async Task HandleEventAsync(TransactionCreatingEventEto eventData)
        {
            await UpdateAccountBalance(eventData.Entity);
        }

        public async Task HandleEventAsync(TransactionUpdatedEventEto eventData)
        {
            await UpdateAccountBalance(eventData.Entity);
        }

        [UnitOfWork]
        private async Task UpdateAccountBalance(Transaction transaction)
        {
            try
            {
                Account accountFrom = _accounts.Where(x => x.Id == transaction.AccountFromId).SingleOrDefault();
                Account accountTo = _accounts.Where(x => x.Id == transaction.AccountToId).SingleOrDefault();
                if (accountFrom != null)
                {
                    var income = _transactions.Where(x => x.AccountToId == accountFrom.Id && x.TransactionState).Sum(x => x.Amount);
                    var expenditure = _transactions.Where(x => x.AccountFromId == accountFrom.Id && x.TransactionState).Sum(x => x.Amount);
                    accountFrom.Balance = income - expenditure - transaction.Amount;
                    await _accounts.UpdateAsync(accountFrom);
                }
                if (accountTo != null)
                {
                    var income = _transactions.Where(x => x.AccountToId == accountTo.Id && x.TransactionState).Sum(x => x.Amount);
                    var expenditure = _transactions.Where(x => x.AccountFromId == accountTo.Id && x.TransactionState).Sum(x => x.Amount);
                    accountTo.Balance = income - expenditure + (transaction.Amount * transaction.ExchangeRate);
                    await _accounts.UpdateAsync(accountTo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}