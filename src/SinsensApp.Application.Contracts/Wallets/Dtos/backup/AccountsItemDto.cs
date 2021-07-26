using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    public class AccountsItemDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int model_state { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int sync_state { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string currency_code { get; set; }

        /// <summary>
        /// 广发证券
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 总余额，包含股票，资金
        /// </summary>
        public string note { get; set; }

        /// <summary>
        ///
        /// </summary>
        public decimal balance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string include_in_totals { get; set; }
    }
}