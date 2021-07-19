using SinsensApp.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SinsensApp.Wallets
{
    /// <summary>
    /// 账户
    /// </summary>
    [Table("WalletAccounts")]
    public class Account : FullAuditedEntity<Guid>, IMultiTenant, IMustHasUser<Guid>
    {
        public Account()
        {
        }

        public virtual Currency Currency { get; set; }

        public Guid? TenantId { get; set; }

        /// <summary>
        /// 同步状态：1-已同步
        /// </summary>
        public bool SyncState { get; set; }

        /// <summary>
        /// 货币代码
        /// </summary>
        [ForeignKey("Currency")]
        [StringLength(WalletsConsts.MaxTitleLength)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        //public virtual Currency Currency { get; set; }

        /// <summary>
        /// 账号名称
        /// </summary>
        [StringLength(WalletsConsts.MaxTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 是否包含在总计
        /// </summary>
        public bool IncludeInTotals { get; set; }

        public Guid UserId { get; set; }
    }
}