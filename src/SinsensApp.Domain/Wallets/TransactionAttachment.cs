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
    /// 交易附件
    /// </summary>
    [Table("WalletTransactionAttachments")]
    public class TransactionAttachment : FullAuditedEntity<Guid>, IMultiTenant, IMustHasUser<Guid>
    {
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 同步状态：1-已同步
        /// </summary>
        public bool SyncState { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(Wallets.WalletsConsts.MaxTitleLength)]
        public string Title { get; set; }

        public long Size { get; set; }

        public int Index { get; set; }

        [StringLength(Wallets.WalletsConsts.MaxUrlStringLength)]
        public string Url { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}