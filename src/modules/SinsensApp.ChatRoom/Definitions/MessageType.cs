using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom.Definitions
{
    public enum MessageType
    {
        [Description("文本消息")]
        Text = 0,

        [Description("图片")]
        Picture = 1,

        [Description("声音")]
        Audio = 2,

        [Description("视频")]
        Video = 3,

        [Description("文档")]
        Document = 4,

        [Description("文件")]
        File = 5,

        [Description("链接")]
        Url = 6
    }
}