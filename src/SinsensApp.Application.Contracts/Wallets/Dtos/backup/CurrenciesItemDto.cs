using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    public class CurrenciesItemDto
    {
        /// <summary>
        ///
        /// </summary>
        public string id { get; set; }

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
        public string code { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int symbol_position { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string decimal_separator { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string group_separator { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int decimal_count { get; set; }
    }
}