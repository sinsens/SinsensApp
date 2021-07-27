using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.statics
{
    public class PeriodExpenseStatRequestDto
    {
        /// <summary>
        /// 缓存时间
        /// </summary>
        public const int CacheMinute = 5;

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}