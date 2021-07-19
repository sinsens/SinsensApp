using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CreateUpdateAccountDto
    {
        [Required(ErrorMessage = "请选择货币")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "请输入主题")]
        public string Title { get; set; }

        public string Note { get; set; }

        [Range(-99999999, 9999999999999, ErrorMessage = "金额应在 -99999999-9999999999999 之间")]
        public decimal? Balance { get; set; }

        public bool? IncludeInTotals { get; set; }
    }
}