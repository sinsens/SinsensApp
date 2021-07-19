using System;
using SinsensApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public class AccountRepository : EfCoreRepository<SinsensAppDbContext, Account, Guid>, IAccountRepository
    {
        public AccountRepository(IDbContextProvider<SinsensAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}