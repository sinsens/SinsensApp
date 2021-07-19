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
    [Table("WalletCurrencyRates")]
    public class CurrencyRate : Entity<string>
    {
        public CurrencyRate()
        {
            Rates = new List<Rate>();
        }

        public CurrencyRate(string code) : this()
        {
            Id = code;
        }

        /// <summary>
        /// 货币代码
        /// </summary>
        [Key]
        [Required]
        [StringLength(WalletsConsts.MaxTitleLength)]
        [ForeignKey("Currency")]
        public string Code { get; set; }

        public virtual Currency Currency { get; set; }

        public IEnumerable<Rate> Rates { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [StringLength(WalletsConsts.MaxNormalStringLength)]
        public string Date { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        public long? Last_Updated { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Provider { get; set; }
    }

    [Table("WalletCurrencyRateDetails")]
    public class Rate : AuditedEntity<string>
    {
        public Rate()
        {
        }

        public Rate(string fromCode, string toCode, decimal ratio = 1)
        {
            Id = $"{fromCode}_{toCode}";
            FromCode = fromCode;
            ToCode = toCode;
            Ratio = ratio;
        }

        [Required]
        [StringLength(WalletsConsts.MaxTitleLength)]
        public string FromCode { get; set; }

        /// <summary>
        /// 目标货币
        /// </summary>
        [Required]
        [StringLength(WalletsConsts.MaxTitleLength)]
        public string ToCode { get; set; }

        /// <summary>
        /// 比值
        /// </summary>
        public decimal Ratio { get; set; }
    }
}