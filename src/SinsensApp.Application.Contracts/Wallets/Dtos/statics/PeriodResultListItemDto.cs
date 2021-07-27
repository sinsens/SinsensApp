using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.statics
{
    public class PeriodResultListItemDto
    {
        public PeriodResultListItemDto()
        {
            Children = new HashSet<PeriodResultListItemDto>();
        }

        public int Color { get; set; }

        public string Text { get; set; }

        public decimal Value { get; set; }

        public float Percent { get; set; }

        public HashSet<PeriodResultListItemDto> Children { get; set; }
    }
}