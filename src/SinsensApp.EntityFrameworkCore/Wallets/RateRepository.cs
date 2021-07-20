using System;
using SinsensApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public class RateRepository : EfCoreRepository<SinsensAppDbContext, Rate, string>, IRateRepository
    {
        public RateRepository(IDbContextProvider<SinsensAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}