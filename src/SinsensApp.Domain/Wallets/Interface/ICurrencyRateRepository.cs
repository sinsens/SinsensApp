using System;
using Volo.Abp.Domain.Repositories;

namespace SinsensApp.Wallets
{
    public interface ICurrencyRateRepository : IRepository<CurrencyRate, string>
    {
    }
}