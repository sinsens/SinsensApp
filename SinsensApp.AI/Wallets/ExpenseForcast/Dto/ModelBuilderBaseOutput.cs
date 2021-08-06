using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus;

namespace SinsensApp.AI.Event.Eto
{
    [EventName(AIConst.ModelBuilderConst.Finish)]
    public class ModelBuilderBaseOutput
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public Guid? TenantID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Start { get; } = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// 执行耗时
        /// </summary>
        public double ExecuteTime
        {
            get
            {
                if (End != default)
                {
                    return (End - Start).TotalMilliseconds;
                }
                return (DateTime.Now - Start).TotalMilliseconds;
            }
        }
    }
}