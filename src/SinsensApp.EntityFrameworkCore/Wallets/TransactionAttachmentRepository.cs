using System;
using SinsensApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public class TransactionAttachmentRepository : EfCoreRepository<SinsensAppDbContext, TransactionAttachment, Guid>, ITransactionAttachmentRepository
    {
        public TransactionAttachmentRepository(IDbContextProvider<SinsensAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}