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

        [Required(ErrorMessage = "��ѡ�������")]
        public CategoryDto Category { get; set; }

        public ICollection<string> TagIds { get; set; }

        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "�����뽻�׽��")]
        [Range(-99999999, 999999999999, ErrorMessage = "���׽��Ӧ�� -99999999~999999999999 ֮��")]
        public decimal Amount { get; set; }

        /// <summary>
        /// ���ʣ�ת��ʱ�õ�
        /// </summary>
        public decimal? ExchangeRate { get; set; }

        public string Note { get; set; }

        /// <summary>
        /// ����״̬��true = ��ȷ��
        /// </summary>
        public bool? TransactionState { get; set; }

        [Required(ErrorMessage = "��ѡ��������")]
        public TransactionType TransactionType { get; set; }

        public bool? IncludeInReports { get; set; }

        public List<TransactionAttachmentDto> Attachments { get; set; }
    }
}