using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.statics
{
    public class PeriodExpenseStatRequestDto
    {
        public PeriodType PeriodType { get; set; } = PeriodType.Monthly;
    }
}