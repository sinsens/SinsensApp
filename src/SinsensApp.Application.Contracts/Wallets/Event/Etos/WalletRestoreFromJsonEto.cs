using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace SinsensApp.Wallets.Event
{
    public class WalletRestoreFromJsonEto
    {
        public WalletRestoreFromJsonEto(Guid userId, Guid? tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }

        public Guid? TenantId { get; protected set; }

        [Required]
        public Guid? UserId { get; protected set; }
    }
}