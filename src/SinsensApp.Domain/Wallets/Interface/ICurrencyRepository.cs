using System;
using Volo.Abp.Domain.Repositories;

namespace SinsensApp.Wallets
{
    public interface ICurrencyRepository : IRepository<Currency, string>
    {
    }
}