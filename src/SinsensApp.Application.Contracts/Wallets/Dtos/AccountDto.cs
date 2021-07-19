using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ExceptionHandling;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class AccountDto : EntityDto<Guid>
    {
        public string CurrencyCode { get; set; }

        public CurrencyDto Currency { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public decimal? Balance { get; set; }

        public bool? IncludeInTotals { get; set; }
    }
}