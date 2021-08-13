using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom.Domain
{
    public class ChatRoom
    {
        public ChatRoom()
        {
            Managers = new List<RoomUser>();
            Users = new List<RoomUser>();
            Id = Guid.NewGuid().ToString("n");
            userClients = new HashSet<string>();
        }

        private HashSet<string> userClients;

        public void Login(RoomUser user)
        {
            if (Users.Any(x => x.Id == user.Id) == false)
            {
                user.Online = true;
                Users.Add(user);
            }
            else
            {
                user.Online = true;
            }
        }

        public void Leave(string clientId)
        {
            var idx = Users.FindIndex(x => x.Id == clientId);
            if (idx > -1)
            {
                Users.RemoveAt(idx);
            }
        }

        public IEnumerable<string> Clients
        {
            get
            {
                userClients.Clear();
                foreach (var user in Users)
                {
                    if (user.Online)
                        userClients.Add(user.Id);
                }
                foreach (var user in Managers)
                {
                    if (user.Online)
                        userClients.Add(user.Id);
                }
                if (Owner != null && Owner.Online)
                    userClients.Add(Owner.Id);
                return userClients;
            }
        }

        /// <summary>
        /// 房间名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 房间号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 房主
        /// </summary>
        public RoomUser Owner { get; set; }

        /// <summary>
        /// 房管
        /// </summary>
        public List<RoomUser> Managers { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public List<RoomUser> Users { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }

    public class RoomUser : IRoomUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }
    }
}