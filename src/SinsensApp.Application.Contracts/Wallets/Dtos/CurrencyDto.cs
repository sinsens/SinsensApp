using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class CurrencyDto : EntityDto<string>
    {
        public string Code { get; set; }

        public IEnumerable<RateDto> CurrencyRate { get; set; }

        public string Symbol { get; set; }

        public SymbolPositionType SymbolPosition { get; set; }

        public string DecimalSeparator { get; set; }

        public string GroupSeparator { get; set; }

        public int DecimalCount { get; set; }
    }
}