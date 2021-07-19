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
    /// 标签
    /// </summary>
    [Table("WalletTags")]
    public class Tag : FullAuditedEntity<Guid>, IMultiTenant, IMustHasUser<Guid>
    {
        public Tag()
        {
            Transactions = new List<Transaction>();
        }

        public Guid UserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 同步状态：1-已同步
        /// </summary>
        public bool SyncState { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(WalletsConsts.MaxTitleLength)]
        public string Title { get; set; }
    }
}