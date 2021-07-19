using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CreateUpdateAccountDto
    {
        [Required(ErrorMessage = "��ѡ�����")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "����������")]
        public string Title { get; set; }

        public string Note { get; set; }

        [Range(-99999999, 9999999999999, ErrorMessage = "���Ӧ�� -99999999-9999999999999 ֮��")]
        public decimal? Balance { get; set; }

        public bool? IncludeInTotals { get; set; }
    }
}