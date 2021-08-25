using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR;
using SinsensApp.ChatRoom.Domain.Dto;
using Volo.Abp.ObjectMapping;

namespace SinsensApp.ChatRoom.Domain
{
    //[HubRoute("/hub/chatroom")]
    public class ChatRoomHub : Hub, IChatRoomHub
    {
        private readonly IChatRoomManager _roomManager;

        private readonly IChatRoomMassageManager _massageManager;

        private readonly IObjectMapper _objectMapper;

        public ChatRoomHub(
            IChatRoomManager roomManager,
            IChatRoomMassageManager massageManager,
            IObjectMapper objectMapper)
        {
            _roomManager = roomManager;
            _massageManager = massageManager;
            _objectMapper = objectMapper;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("OnConnected", Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return Task.Run(() =>
            {
                var user = _roomManager.FindUser(Context.ConnectionId);
                if (user is IRoomUser)
                {
                    user.Online = false;
                    _roomManager.SaveData();
                }
            });
        }

        public virtual async Task<ResponseMessage> JoinRoomAsync(string roomName, string password = null)
        {
            var room = _roomManager.GetChatRoom(roomName);
            if (room is ChatRoom)
            {
                var user = _roomManager.FindUser(Context.ConnectionId);
                if (user == null)
                {
                    return ResponseMessage.ResponseError("登录已过期，请重新登录");
                }
                if (room.Clients.Contains(user.Id))
                {
                    return ResponseMessage.ResponseOk();
                }
                var result = _roomManager.JoinRoom(roomName, Context.ConnectionId, password);
                if (result.isOk)
                {
                    var message = new Message { RoomId = room.Id, Name = user.Name, MessagePublisherType = Definitions.MessagePublisherType.System, Value = $"{user.Name} 进入房间" };
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    await Clients.Clients(_roomManager.GetChatRoom(roomName).Clients).SendAsync("AddUserMsg", message);
                    return ResponseMessage.ResponseOk();
                }
                else
                {
                    return ResponseMessage.ResponseError(result.message);
                }
            }
            return ResponseMessage.ResponseError("房间名称不存在");
        }

        public virtual async Task<ResponseMessage> CreateRoomAsync(string roomName, string password = null)
        {
            var user = _roomManager.FindUser(Context.ConnectionId);
            if (user == null)
            {
                return ResponseMessage.ResponseError("登录已过期，请重新登录");
            }
            var result = _roomManager.CreateRoom(roomName, Context.ConnectionId, password);
            if (result.isOk == false)
            {
                return ResponseMessage.ResponseError(result.message);
            }
            _roomManager.CreateRoom(roomName, Context.ConnectionId);
            var message = new Message { MessagePublisherType = Definitions.MessagePublisherType.System, Value = "" };
            await Clients.All.SendAsync("RefreshRooms", message); // 刷新房间信息
            return ResponseMessage.ResponseOk();
        }

        public virtual async Task<ResponseMessage> AddUserMsgAsync(string roomName, string msg)
        {
            var cid = Context.ConnectionId;
            // 向所有用户发送消息
            if (_roomManager.HasJoinRoom(roomName, cid) == false)
            {
                return ResponseMessage.ResponseError("未加入该房间");
            }
            var user = _roomManager.FindUser(roomName, cid);
            var room = _roomManager.GetChatRoom(roomName);
            var message = new Message()
            {
                RoomId = room.Id,
                Value = msg,
                Name = user.Name,
                MessagePublisherType = Definitions.MessagePublisherType.User
            };

            await Clients.Clients(_roomManager.GetChatRoom(roomName).Clients).SendAsync("AddUserMsg", message);
            await _massageManager.AddMessageAsync(room.Id, message);
            return ResponseMessage.ResponseOk();
        }

        public virtual async Task<IEnumerable<IMessage>> GetMessagesAsync(string roomName)
        {
            var room = _roomManager.GetChatRoom(roomName);
            return await _massageManager.GetAllMessages(room.Id);
        }

        public virtual async Task<IEnumerable<IMessage>> GetMessagesRangeAsync(string roomName, int index, int count)
        {
            var room = _roomManager.GetChatRoom(roomName);
            return await _massageManager.GetMessages(room.Id, index, count);
        }

        public virtual async Task<IEnumerable<ChatRoomDto>> GetChatRoomsAsync()
        {
            return await Task.Run(() =>
            {
                var rooms = _roomManager.Rooms;
                return _objectMapper.Map<IList<ChatRoom>, List<ChatRoomDto>>(rooms);
            });
        }

        public virtual async Task LeaveOutRoomAsync(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            _roomManager.LeaveRoom(roomName, Context.ConnectionId);
        }

        public virtual async Task ReConnectedAsync(string clientId)
        {
            await Task.Run(() => _roomManager.ReConnect(clientId, Context.ConnectionId));
        }

        public virtual async Task<ResponseMessage> UpdateMyNameAsync(string nickName)
        {
            return await Task.Run(() =>
            {
                var user = _roomManager.FindUser(Context.ConnectionId);
                if (user != null)
                {
                    user.Name = nickName;
                }
                else
                {
                    _roomManager.Login(Context.ConnectionId, nickName);
                }
                return ResponseMessage.ResponseOk("更新成功");
            });
        }
    }
}