using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace SinsensApp.Wallets.Dtos.statics
{
    public class PeriodExpenseStatRequestResultDto
    {
        public PeriodExpenseStatRequestResultDto()
        {
            Items = new HashSet<PeriodResultListItemDto>();
        }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal Total { get; set; }

        public ICollection<PeriodResultListItemDto> Items { get; set; }
    }
}