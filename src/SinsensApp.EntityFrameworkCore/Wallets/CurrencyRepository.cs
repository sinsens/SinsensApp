using System;
using SinsensApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public class CurrencyRepository : EfCoreRepository<SinsensAppDbContext, Currency, string>, ICurrencyRepository
    {
        public CurrencyRepository(IDbContextProvider<SinsensAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}