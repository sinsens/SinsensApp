using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class TransactionDto : EntityDto<Guid>
    {
        public AccountDto AccountFrom { get; set; }

        public Guid? AccountFromId { get; set; }
        public Guid? AccountToId { get; set; }

        public Guid? CategoryId { get; set; }

        public AccountDto AccountTo { get; set; }

        public CategoryDto Category { get; set; }

        public ICollection<TagDto> Tags { get; set; }

        public DateTime? Date { get; set; }

        public decimal Amount { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string Note { get; set; }

        public bool TransactionState { get; set; }

        public TransactionType TransactionType { get; set; }

        public string TransactionTypeDescription { get; set; }

        public bool IncludeInReports { get; set; }

        public List<TransactionAttachmentDto> Attachments { get; set; }
    }
}