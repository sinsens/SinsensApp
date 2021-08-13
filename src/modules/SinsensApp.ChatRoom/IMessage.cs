using SinsensApp.ChatRoom.Definitions;
using System;

namespace SinsensApp.ChatRoom
{
    public interface IMessage
    {
        MessagePublisherType MessagePublisherType { get; set; }
        MessageType MessageType { get; }

        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomId { get; }

        string Name { get; }
        string SendAt { get; }
        string Value { get; }
    }
}