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

        /// <summary>
        /// 支出
        /// </summary>
        public decimal Expenditure { get; set; }

        /// <summary>
        /// 收入
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// 转账
        /// </summary>
        public decimal Transfer { get; set; }

        public HashSet<PeriodResultListItemDto> Items { get; set; }
    }
}