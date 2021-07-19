using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.Wallets
{
    public class WalletsConsts
    {
        /// <summary>
        /// 最大名称长度
        /// </summary>
        public const int MaxTitleLength = 32;

        /// <summary>
        /// 最大常规文本字段长度
        /// </summary>
        public const int MaxNormalStringLength = 128;

        /// <summary>
        /// 最大 URL 字符串长度
        /// </summary>
        public const int MaxUrlStringLength = 255;

        /// <summary>
        /// 最大备注字段长度
        /// </summary>
        public const int MaxNoteStringLength = 2000;

        public const string DefaultCurrencyCode = "CNY";

        public const string DefaultCurrencyRateApiBaseUrl = "https://api.exchangerate-api.com/v4/latest/";
    }
}