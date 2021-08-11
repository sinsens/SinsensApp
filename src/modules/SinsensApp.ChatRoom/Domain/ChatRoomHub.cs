using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace SinsensApp.ChatRoom.Domain
{
    //[HubRoute("/hub/chatroom")]
    public class ChatRoomHub : Hub, IChatRoomHub
    {
        private readonly IChatRoomManager _roomManager;

        private readonly IChatRoomMassageManager _massageManager;

        public ChatRoomHub(
            IChatRoomManager roomManager,
            IChatRoomMassageManager massageManager)
        {
            _roomManager = roomManager;
            _massageManager = massageManager;
        }

        public virtual async Task<ResponseMessage> JoinGroup(string roomName, string userName)
        {
            if (_roomManager.IsExist(roomName))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                var msg = new Message { Name = userName, MessagePublisherType = Definitions.MessagePublisherType.System, Value = $"{userName} 进入房间" };
                await Clients.Group(roomName).SendAsync("AddSysMsg", msg);
                return ResponseMessage.ResponseOk();
            }
            return ResponseMessage.ResponseError("房间名称不存在");
        }

        public virtual async Task<ResponseMessage> CreateRoom(string roomName)
        {
            if (_roomManager.IsExist(roomName))
            {
                return ResponseMessage.ResponseError("房间名称已存在");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            _roomManager.CreateRoom(roomName, Context.ConnectionId);
            return ResponseMessage.ResponseOk();
        }

        public virtual async Task<ResponseMessage> AddUserMsg(string roomName, string msg)
        {
            var cid = Context.ConnectionId;

            var message = new Message()
            {
                Value = msg,
                Name = cid,
                MessagePublisherType = Definitions.MessagePublisherType.User
            };
            // 向所有用户发送消息
            if (_roomManager.HasJoinRoom(roomName, cid) == false)
            {
                return ResponseMessage.ResponseError("未加入该房间");
            }
            await Clients.Groups(roomName).SendAsync("AddUserMsg", message);
            await _massageManager.AddMessageAsync(message);
            return ResponseMessage.ResponseOk();
        }

        public virtual async Task<IEnumerable<IMessage>> GetMessagesAsync()
        {
            return await _massageManager.GetAllMessages();
        }

        public virtual async Task<IEnumerable<IMessage>> GetMessagesRangeAsync(int index, int count)
        {
            return await _massageManager.GetMessages(index, count);
        }

        public virtual IEnumerable<ChatRoom> GetChatRooms()
        {
            return _roomManager.Rooms;
        }

        public virtual void LeaveOutRoom(string roomName)
        {
            _roomManager.LeaveRoom(roomName, Context.ConnectionId);
        }
    }
}