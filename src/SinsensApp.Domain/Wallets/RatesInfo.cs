using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.Wallets
{
    public class RatesInfo
    {
        /// <summary>
        ///
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string WARNING_UPGRADE_TO_V6 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string terms { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string date { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long time_last_updated { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IDictionary<string, decimal> rates { get; set; }
    }
}