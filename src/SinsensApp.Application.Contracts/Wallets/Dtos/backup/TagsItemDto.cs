using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    public class TagsItemDto
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
        /// 花呗
        /// </summary>
        public string title { get; set; }
    }
}