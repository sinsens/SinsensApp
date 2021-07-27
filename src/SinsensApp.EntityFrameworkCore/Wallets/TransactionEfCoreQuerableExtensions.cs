using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public static class TransactionEfCoreQueryableExtensions
    {
        public static IQueryable<Transaction> IncludeDetails(this IQueryable<Transaction> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Category)
                .Include(x => x.Tags)
                // .Include(x => x.xxx) // TODO: AbpHelper generated
                ;
        }
    }
}