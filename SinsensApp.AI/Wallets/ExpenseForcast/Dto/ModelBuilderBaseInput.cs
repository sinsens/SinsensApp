using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.AI.Event.Eto
{
    public class ModelBuilderBaseInput
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public Guid? TenantID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserID { get; set; }
    }
}