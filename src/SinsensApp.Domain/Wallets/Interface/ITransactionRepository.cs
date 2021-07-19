using System;
using Volo.Abp.Domain.Repositories;

namespace SinsensApp.Wallets
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
    }
}