using System;
using SinsensApp.Wallets;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CreateUpdateTransactionDto
    {
        public CreateUpdateTransactionDto()
        {
            Tags = new HashSet<TagDto>();
            Attachments = new List<TransactionAttachmentDto>();
        }

        public CategoryDto Category { get; set; }

        public AccountDto AccountFrom { get; set; }

        public AccountDto AccountTo { get; set; }

        public ICollection<TagDto> Tags { get; set; }

        [Required(ErrorMessage = "请选择交易时间")]
        public string Date { get; set; }

        [Required(ErrorMessage = "请输入交易金额")]
        [Range(-99999999, 999999999999, ErrorMessage = "交易金额应在 -99999999~999999999999 之间")]
        public decimal Amount { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string Note { get; set; }

        public bool? TransactionState { get; set; }

        [Required(ErrorMessage = "请选择交易类型")]
        public TransactionType TransactionType { get; set; }

        public bool? IncludeInReports { get; set; }

        public List<TransactionAttachmentDto> Attachments { get; set; }
    }
}