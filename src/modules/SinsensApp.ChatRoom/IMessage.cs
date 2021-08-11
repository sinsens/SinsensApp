using SinsensApp.ChatRoom.Definitions;
using System;

namespace SinsensApp.ChatRoom
{
    public interface IMessage
    {
        MessagePublisherType MessagePublisherType { get; set; }
        MessageType MessageType { get; }
        string Name { get; set; }
        DateTime SendAt { get; set; }
        string Value { get; set; }
    }
}