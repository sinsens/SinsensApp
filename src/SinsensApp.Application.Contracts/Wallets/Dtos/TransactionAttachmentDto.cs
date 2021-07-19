using System;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class TransactionAttachmentDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }

        public long Size { get; set; }

        public int Index { get; set; }

        public string Url { get; set; }
    }
}