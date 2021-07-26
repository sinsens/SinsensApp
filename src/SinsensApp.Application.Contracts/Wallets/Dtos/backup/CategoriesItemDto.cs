using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    public class CategoriesItemDto
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
        /// 餐饮
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int color { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int transaction_type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int sort_order { get; set; }
    }
}