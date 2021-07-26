using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.Wallets
{
    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// 支出
        /// </summary>
        [Description("支出")]
        Expenditure = 1,

        /// <summary>
        /// 收入
        /// </summary>
        [Description("收入")]
        Income = 2,

        /// <summary>
        /// 转账（内部交易）
        /// </summary>
        [Description("转账")]
        Transfer = 3
    }

    /// <summary>
    /// 符号位置
    /// </summary>
    public enum SymbolPositionType
    {
        /// <summary>
        /// 左侧
        /// </summary>
        Left = 1,

        /// <summary>
        /// 右侧
        /// </summary>
        Right = 2
    }

    /// <summary>
    /// 统计周期
    /// </summary>
    [Description("统计周期")]
    public enum PeriodType
    {
        /// <summary>
        /// 每周
        /// </summary>
        [Description("每周")]
        Weekly = 1,

        /// <summary>
        /// 每月
        /// </summary>
        [Description("每月")]
        Monthly = 2,

        /// <summary>
        /// 每年
        /// </summary>
        [Description("每年")]
        Annually = 3,
    }
}