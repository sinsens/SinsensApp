using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SinsensApp.Wallets.Dtos
{
    public class RateDto : EntityDto
    {
        /// <summary>
        /// 目标货币
        /// </summary>
        public string ToCode { get; set; }

        /// <summary>
        /// 比值
        /// </summary>
        public decimal Ratio { get; set; }
    }
}