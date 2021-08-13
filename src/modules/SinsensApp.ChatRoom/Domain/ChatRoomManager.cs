using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace SinsensApp.ChatRoom.Domain
{
    public class ChatRoomManager : IChatRoomManager, IDisposable
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDatabase _database;
        private readonly System.Threading.Timer _timer;

        public ChatRoomManager(IConnectionMultiplexer connection, IJsonSerializer serializer)
        {
            _connectionMultiplexer = connection;
            _jsonSerializer = serializer;
            _database = connection.GetDatabase(2);
            LoadData();
            _timer = new System.Threading.Timer((callback) =>
            {
                SaveData(); // 启动后每 10 分钟保存一次数据
            }, _database, 600000, 600000);
        }

        public IList<ChatRoom> Rooms { protected set; get; }
        public IList<RoomUser> Users { get; protected set; }

        public virtual void SaveData()
        {
            _database.StringSet("Users", _jsonSerializer.Serialize(Users));
            _database.StringSet("Rooms", _jsonSerializer.Serialize(Rooms));
        }

        private void LoadData()
        {
            var rooms = _database.StringGet("Rooms");
            if (rooms.HasValue)
            {
                Rooms = _jsonSerializer.Deserialize<List<ChatRoom>>(rooms);
            }
            var users = _database.StringGet("Users");
            if (users.HasValue)
            {
                Users = _jsonSerializer.Deserialize<List<RoomUser>>(users);
            }
            if (Rooms == null)
            {
                Rooms = new List<ChatRoom>(10);
                Rooms.Add(new ChatRoom { Name = "公开房间" });
            }
            if (Users == null)
            {
                Users = new List<RoomUser>(10);
            }
        }

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
            Rooms.Add(new ChatRoom { Owner = FindUser(ownerId), Name = roomName });
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
        public (bool isOk, string message) JoinRoom(string roomName, string clientId)
        {
            var room = GetChatRoom(roomName);
            var user = FindUser(clientId);
            if (room == null)
            {
                return (false, "该房间不存在");
            }
            else if (room.Users.Any(x => x.Id == clientId))
            {
                return (true, "您已加入该房间");
            }
            else if (room.Users.Any(x => x.Name == user.Name))
            {
                return (false, "该房间已存在同名用户");
            }
            room.Login(user);

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

        public RoomUser FindUser(string clientId)
        {
            return Users.Where(x => x.Id == clientId).FirstOrDefault();
        }

        public RoomUser Login(string clientId, string nickName)
        {
            var user = FindUser(clientId);
            if (user == null)
            {
                user = new RoomUser
                {
                    Id = clientId,
                    Name = nickName,
                    Online = true
                };
            }
            else
            {
                user.Online = true;
            }
            Users.Add(user);
            return user;
        }

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public RoomUser FindUser(string roomName, string clientId)
        {
            var room = GetChatRoom(roomName);
            if (room != null)
            {
                return room.Users.Where(x => x.Id == clientId).FirstOrDefault();
            }
            return FindUser(clientId);
        }

        public void ReConnect(string oldClientId, string newClientId)
        {
            var user = FindUser(oldClientId);
            if (user != null)
            {
                user.Id = newClientId;
                user.Online = true;
            }
        }

        public void Dispose()
        {
            if (_connectionMultiplexer.IsConnected)
                _connectionMultiplexer.Close();
        }
    }
}