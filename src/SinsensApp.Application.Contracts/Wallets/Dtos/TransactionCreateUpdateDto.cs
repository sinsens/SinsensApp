using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class TransactionCreateUpdateDto
    {
        public Guid? AccountFromId { get; set; }

        public Guid? AccountToId { get; set; }

        [Required(ErrorMessage = "请选择交易类别")]
        public CategoryDto Category { get; set; }

        public ICollection<string> TagIds { get; set; }

        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "请输入交易金额")]
        [Range(-99999999, 999999999999, ErrorMessage = "交易金额应在 -99999999~999999999999 之间")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 汇率，转账时用到
        /// </summary>
        public decimal? ExchangeRate { get; set; }

        public string Note { get; set; }

        /// <summary>
        /// 交易状态：true = 已确认
        /// </summary>
        public bool? TransactionState { get; set; }

        [Required(ErrorMessage = "请选择交易类型")]
        public TransactionType TransactionType { get; set; }

        public bool? IncludeInReports { get; set; }

        public List<TransactionAttachmentDto> Attachments { get; set; }
    }
}