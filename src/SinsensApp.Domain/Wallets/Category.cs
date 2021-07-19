using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SinsensApp.Wallets
{
    /// <summary>
    /// 分类
    /// </summary>
    [Table("WalletCategories")]
    public class Category : FullAuditedEntity<Guid>, IMultiTenant, IMustHasUser<Guid>
    {
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 同步状态：1-已同步
        /// </summary>
        public bool SyncState { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [StringLength(WalletsConsts.MaxTitleLength)]
        public string Title { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; }
    }
}