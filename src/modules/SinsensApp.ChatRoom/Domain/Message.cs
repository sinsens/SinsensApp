using SinsensApp.ChatRoom.Definitions;
using SinsensApp.ChatRoom.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom.Domain
{
    public class Message : IMessage
    {
        public string Name { get; set; }
        public DateTime SendAt { get; set; } = DateTime.Now;
        public string Value { get; set; }
        public MessageType MessageType => Value.GetMessageType();
        public string RoomId { get; set; }
        public MessagePublisherType MessagePublisherType { get; set; }
    }
}