using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    public class TransactionsItemDto
    {
        public TransactionsItemDto()
        {
            tag_ids = new List<Guid>();
        }

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
        public Guid? account_from_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Guid? account_to_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Guid? category_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<Guid> tag_ids { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long date { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public decimal exchange_rate { get; set; }

        /// <summary>
        /// 科目三私人教练练车费
        /// </summary>
        public string note { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int transaction_state { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int transaction_type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string include_in_reports { get; set; }
    }
}