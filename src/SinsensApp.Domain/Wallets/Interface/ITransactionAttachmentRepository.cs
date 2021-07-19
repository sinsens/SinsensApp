using System;
using Volo.Abp.Domain.Repositories;

namespace SinsensApp.Wallets
{
    public interface ITransactionAttachmentRepository : IRepository<TransactionAttachment, Guid>
    {
    }
}