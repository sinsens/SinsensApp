using SinsensApp.Wallets.Dtos.statics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.Wallets
{
    public interface IStatAppService
    {
        /// <summary>
        /// 获取周期统计列表
        /// </summary>
        /// <param name="expenseStatRequestDto"></param>
        /// <returns></returns>
        Task<PeriodExpenseStatRequestResultDto> GetPeriodResultList(PeriodExpenseStatRequestDto input);
    }
}