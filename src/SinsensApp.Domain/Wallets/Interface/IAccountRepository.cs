using System;
using Volo.Abp.Domain.Repositories;

namespace SinsensApp.Wallets
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {
    }
}