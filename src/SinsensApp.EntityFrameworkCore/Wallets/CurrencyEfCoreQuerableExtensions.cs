using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public static class CurrencyEfCoreQueryableExtensions
    {
        public static IQueryable<Currency> IncludeDetails(this IQueryable<Currency> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                // .Include(x => x.xxx) // TODO: AbpHelper generated
                ;
        }
    }
}