using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public static class AccountEfCoreQueryableExtensions
    {
        public static IQueryable<Account> IncludeDetails(this IQueryable<Account> queryable, bool include = true)
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