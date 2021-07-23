using SinsensApp.Wallets.Dtos.backup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SinsensApp.Wallets
{
    /// <summary>
    /// 钱包备份、还原服务
    /// </summary>
    public interface IWalletBackupService : IApplicationService
    {
        Task<WalletBackupRequestResultDto> Backup();

        /// <summary>
        /// 从备份中还原
        /// </summary>
        /// <param name="clearBeforeRestore">还原前先清空现有数据:false</param>
        /// <param name="skipIfExists">是否跳过已有记录：true, false=覆盖已有</param>
        /// <returns></returns>
        Task<string> Restore(bool clearBeforeRestore = false, bool skipIfExists = true);
    }
}