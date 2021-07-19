using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SinsensApp.Wallets
{
    /// <summary>
    /// 货币
    /// </summary>
    [Table("WalletCurrencies")]
    public class Currency : Entity<string>
    {
        /// <summary>
        /// 货币代码
        /// </summary>
        [Required]
        [StringLength(WalletsConsts.MaxTitleLength)]
        [Key]
        public string Code { get; set; }

        public virtual CurrencyRate CurrencyRate { get; set; }

        /// <summary>
        /// 货币符号
        /// </summary>
        [StringLength(WalletsConsts.MaxNormalStringLength)]
        public string Symbol { get; set; }

        /// <summary>
        /// 符号位置：1-左，2-右
        /// </summary>
        public SymbolPositionType SymbolPosition { get; set; }

        /// <summary>
        /// 小数点分隔符
        /// </summary>
        [StringLength(WalletsConsts.MaxNormalStringLength)]
        public string DecimalSeparator { get; set; }

        /// <summary>
        /// 分隔符（千分位）
        /// </summary>
        [StringLength(WalletsConsts.MaxNormalStringLength)]
        public string GroupSeparator { get; set; }

        /// <summary>
        /// 小数点
        /// </summary>
        public int DecimalCount { get; set; }
    }
}