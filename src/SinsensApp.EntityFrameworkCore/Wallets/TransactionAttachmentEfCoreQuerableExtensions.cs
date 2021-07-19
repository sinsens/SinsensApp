using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public static class TransactionAttachmentEfCoreQueryableExtensions
    {
        public static IQueryable<TransactionAttachment> IncludeDetails(this IQueryable<TransactionAttachment> queryable, bool include = true)
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