using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.ChatRoom.Domain
{
    public class ChatRoomManager : IChatRoomManager
    {
        public ChatRoomManager()
        {
            Rooms = new List<ChatRoom>(10);
        }

        public IList<ChatRoom> Rooms { protected set; get; }

        public bool IsExist(string roomName)
        {
            return Rooms.Any(x => string.Equals(x.Name, roomName, StringComparison.InvariantCultureIgnoreCase));
        }

        public (bool isOk, string message) CreateRoom(string roomName, string ownerId)
        {
            if (IsExist(roomName))
            {
                return (false, "房间已存在");
            }
            Rooms.Add(new ChatRoom { OwnerId = ownerId, Name = roomName });
            return (true, "创建成功");
        }

        /// <summary>
        /// 是否已加入
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool HasJoinRoom(string roomName, string clientId)
        {
            if (IsExist(roomName) == false)
            {
                return false;
            }
            return Rooms.Any(x => x.Users.Any(u => u.Id == clientId));
        }

        /// <summary>
        /// 加入
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="clientId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public (bool isOk, string message) JoinRoom(string roomName, string clientId, string name)
        {
            var room = GetChatRoom(roomName);
            if (room == null)
            {
                return (false, "该房间不存在");
            }
            else if (room.Users.Any(x => x.Id == clientId))
            {
                return (true, "您已加入该房间");
            }
            else if (room.Users.Any(x => x.Name == name))
            {
                return (false, "已有相同的用户名存在于该房间");
            }
            var chatUser = new RoomUser { Id = clientId, Name = name };
            room.Login(chatUser);

            return (true, "成功加入该房间");
        }

        public bool LeaveRoom(string roomName, string clientId)
        {
            var room = GetChatRoom(roomName);
            if (room != null)
            {
                room.Leave(clientId);
            }
            return true;
        }

        public ChatRoom GetChatRoom(string roomName)
        {
            return Rooms.Where(x => string.Equals(x.Name, roomName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}