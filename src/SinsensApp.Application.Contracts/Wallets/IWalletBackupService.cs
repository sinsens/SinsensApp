using SinsensApp.Wallets.Dtos.backup;
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

        Task<string> Restore(WalletRestoreRequestDto walletRestoreRequest);
    }
}