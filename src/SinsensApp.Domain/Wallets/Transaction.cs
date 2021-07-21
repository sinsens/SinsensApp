using SinsensApp.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SinsensApp.Wallets
{
    /// <summary>
    /// 交易
    /// </summary>
    [Table("WalletTransactions")]
    public class Transaction : FullAuditedAggregateRoot<Guid>, IMultiTenant, IMustHasUser<Guid>
    {
        public Transaction()
        {
            Tags = new List<Tag>();
            Attachments = new List<TransactionAttachment>();
        }

        public Guid UserId { get; set; }

        public Guid? TenantId { get; set; }

        /// <summary>
        /// 同步状态：1-已同步
        /// </summary>
        public bool SyncState { get; set; }

        [ForeignKey("AccountFrom")]
        public Guid? AccountFromId { get; set; }

        public virtual Account AccountFrom { get; set; }

        [ForeignKey("AccountTo")]
        public Guid? AccountToId { get; set; }

        public virtual Account AccountTo { get; set; }

        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// 标签列表
        /// </summary>
        public virtual ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public decimal ExchangeRate { get; set; } = 1;

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(WalletsConsts.MaxNoteStringLength)]
        public string Note { get; set; }

        /// <summary>
        /// 交易状态：true-已确认
        /// </summary>
        public bool TransactionState { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 是否包含在报表
        /// </summary>
        public bool IncludeInReports { get; set; }

        public virtual ICollection<TransactionAttachment> Attachments { get; set; }
    }
}