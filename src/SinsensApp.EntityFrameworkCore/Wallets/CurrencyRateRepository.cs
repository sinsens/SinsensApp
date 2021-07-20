using SinsensApp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public class CurrencyRateRepository : EfCoreRepository<SinsensAppDbContext, CurrencyRate, string>, ICurrencyRateRepository
    {
        public CurrencyRateRepository(IDbContextProvider<SinsensAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}