using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private const string roomsKey = "ChatRoom:Rooms";
        private const string usersKey = "ChatRoom:Users";
        private static bool onSaving = false;
        private const int saveDelay = 600000;

        public ChatRoomManager(IConnectionMultiplexer connection, IJsonSerializer serializer)
        {
            _connectionMultiplexer = connection;
            _jsonSerializer = serializer;
            _database = connection.GetDatabase(2);
            LoadData();
            _timer = new System.Threading.Timer((callback) =>
            {
                SaveData(); // 启动后每 10 分钟保存一次数据
            }, _database, saveDelay, saveDelay);
        }

        public IList<ChatRoom> Rooms { protected set; get; }
        public IList<RoomUser> Users { get; protected set; }

        public virtual void SaveData()
        {
            if (onSaving) return;
            onSaving = true;

            try
            {
                _timer.Change(int.MaxValue, Timeout.Infinite); // 暂停定时器
                _database.StringSet(usersKey, _jsonSerializer.Serialize(Users));
                _database.StringSet(roomsKey, _jsonSerializer.Serialize(Rooms));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _timer.Change(saveDelay, saveDelay); // 恢复定时器
                onSaving = false;
            }
        }

        private void LoadData()
        {
            var rooms = _database.StringGet(roomsKey);
            var users = _database.StringGet(usersKey);
            if (rooms.HasValue)
            {
                Rooms = _jsonSerializer.Deserialize<List<ChatRoom>>(rooms);
            }
            if (users.HasValue)
            {
                Users = _jsonSerializer.Deserialize<List<RoomUser>>(users);
            }
            if (Rooms == null)
            {
                Rooms = new List<ChatRoom>(10);
                Rooms.Add(new ChatRoom(10000) { Name = "公开房间" });
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

        public (bool isOk, string message) CreateRoom(string roomName, string ownerId, string password = null)
        {
            if (IsExist(roomName))
            {
                return (false, "房间已存在");
            }
            lock (Rooms)
            {
                var roomNumber = 10000 + Rooms.Count;
                var room = new ChatRoom(0);
                room.Owner = FindUser(ownerId);
                room.Name = roomName;
                room.Password = password;

                Rooms.Add(room);
                SaveData();
            }
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
        public (bool isOk, string message) JoinRoom(string roomName, string clientId, string password = null)
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
            else if (room.Users.Any(x => x.Name == user.Name && x.Online))
            {
                return (false, "该房间已存在同名用户");
            }
            else if (!string.IsNullOrWhiteSpace(room.Password) && password != room.Password)
            {
                return (false, "密码错误");
            }
            room.Login(user);
            SaveData();
            return (true, "成功加入该房间");
        }

        public bool LeaveRoom(string roomName, string clientId)
        {
            var room = GetChatRoom(roomName);
            if (room != null)
            {
                room.Leave(clientId);
            }
            SaveData();
            return true;
        }

        public ChatRoom GetChatRoom(string roomName)
        {
            return Rooms.Where(x => string.Equals(x.Name, roomName, StringComparison.InvariantCultureIgnoreCase) || x.Number.ToString() == roomName).FirstOrDefault();
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
            SaveData();
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
            var users = Users.Where(x => x.Id == oldClientId);
            if (users.Any())
            {
                foreach (var user in users)
                {
                    user.Id = newClientId;
                    user.Online = true;
                }
                SaveData();
            }
        }

        public void Dispose()
        {
            if (_connectionMultiplexer.IsConnected)
                _connectionMultiplexer.Close();
        }
    }
}