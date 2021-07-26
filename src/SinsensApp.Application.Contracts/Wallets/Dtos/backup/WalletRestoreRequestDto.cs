using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    /// <summary>
    /// 从 Fiancius 的 json 备份中还原
    /// </summary>
    public class WalletRestoreRequestDto
    {
        /// <summary>
        /// 还原前清理现有数据
        /// </summary>
        [Description("还原前清理现有数据")]
        [Required]
        public bool ClearBeforeRestore { get; set; }

        /// <summary>
        /// 是否跳过相同ID 的记录：true，为 false 时则覆盖已有记录
        /// </summary>
        [Description("是否跳过相同ID 的记录：true，为 false 时则覆盖已有记录")]
        public bool? SkipIfExists { get; set; } = true;
    }
}