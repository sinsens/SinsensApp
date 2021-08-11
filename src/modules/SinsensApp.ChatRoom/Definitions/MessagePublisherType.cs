using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom.Definitions
{
    public enum MessagePublisherType
    {
        [Description("系统")]
        System = 0,

        [Description("房主")]
        Owner = 1,

        [Description("管理员")]
        Manager = 2,

        [Description("用户")]
        User = 3
    }
}