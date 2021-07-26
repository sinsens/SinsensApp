using SinsensApp.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Wallets.Dtos.backup
{
    public class BackupJsonDto
    {
        public BackupJsonDto()
        {
            accounts = new List<AccountsItemDto>();
            categories = new List<CategoriesItemDto>();
            currencies = new List<CurrenciesItemDto>();
            tags = new List<TagsItemDto>();
            transactions = new List<TransactionsItemDto>();
            timestamp = UnixTimeHelper.ToUnixTimeMilliseconds(DateTime.Now);
        }

        /// <summary>
        ///
        /// </summary>
        public int version { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<CurrenciesItemDto> currencies { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<CategoriesItemDto> categories { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<TagsItemDto> tags { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<AccountsItemDto> accounts { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<TransactionsItemDto> transactions { get; set; }
    }
}