using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    public class WalletBackupRequestResultDto
    {
        public const int CacheMinute = 5;

        public string Message { get; set; } = $"{CacheMinute} 分钟内只能执行一次该操作";

        /// <summary>
        /// 文件大小
        /// </summary>
        [Description("文件大小")]
        public long Size { get; set; }

        /// <summary>
        /// 输出路径
        /// </summary>
        [Description("输出路径")]
        public string Url { get; set; }
    }
}