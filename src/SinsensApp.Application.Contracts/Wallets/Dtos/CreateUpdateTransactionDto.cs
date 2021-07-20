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
            TagsIds = new List<string>();
            Attachments = new List<TransactionAttachmentDto>();
        }

        public CategoryDto Category { get; set; }

        public Guid? AccountFromId { get; set; }

        public Guid? AccountToId { get; set; }

        public Guid? CategoryId { get; set; }

        public ICollection<string> TagsIds { get; set; }

        [Required(ErrorMessage = "��ѡ����ʱ��")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "�����뽻�׽��")]
        [Range(-99999999, 999999999999, ErrorMessage = "���׽��Ӧ�� -99999999~999999999999 ֮��")]
        public decimal Amount { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string Note { get; set; }

        public bool? TransactionState { get; set; }

        [Required(ErrorMessage = "��ѡ��������")]
        public TransactionType TransactionType { get; set; }

        public bool? IncludeInReports { get; set; }

        public List<TransactionAttachmentDto> Attachments { get; set; }
    }
}