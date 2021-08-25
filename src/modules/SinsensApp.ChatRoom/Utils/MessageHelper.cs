using NUglify.JavaScript.Syntax;
using SinsensApp.ChatRoom.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom.Utils
{
    public static class MessageHelper
    {
        private readonly static string[] documentsExts = new string[] { "txt", "xls", "doc", "ppt", "md", "pptx", "xlsx", "pptx" };
        private readonly static string[] picsExts = new string[] { "jpg", "png", "bmp", "gif", "jpeg", "svg" };
        private readonly static string[] audiosExts = new string[] { "mp3", "wav", "flac" };
        private readonly static string[] videosExts = new string[] { "mp4", "flv", "mpg", "mkv", "rmvb", "avi", "wmv", "rm" };

        /// <summary>
        /// 通过消息的后缀获取消息类型 .txt
        /// </summary>
        /// <param name="message">[text|url]</param>
        /// <returns></returns>
        public static MessageType GetMessageType(this string message)
        {
            if (message.IsNullOrWhiteSpace())
            {
                return MessageType.Text;
            }
            if (message.ToLowerInvariant().IndexOf("http") < 0)
            {
                return MessageType.Text;
            }

            var ext = message.ToLowerInvariant().Split(".").LastOrDefault();
            if (documentsExts.Contains(ext))
            {
                return MessageType.Document;
            }
            if (picsExts.Contains(ext))
            {
                return MessageType.Picture;
            }
            if (audiosExts.Contains(ext))
            {
                return MessageType.Audio;
            }
            if (videosExts.Contains(ext))
            {
                return MessageType.Video;
            }
            if (message.Split('/').LastOrDefault().Contains('.'))
            {
                return MessageType.File;
            }
            return MessageType.Url;
        }
    }
}