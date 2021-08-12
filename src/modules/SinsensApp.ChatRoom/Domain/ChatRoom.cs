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
            userHasChanges = true;
        }

        private HashSet<string> userClients;
        private bool userHasChanges;

        public void Login(RoomUser user)
        {
            if (Users.Any(x => x.Id == user.Id) == false)
            {
                Users.Add(user);
                userHasChanges = true;
            }
        }

        public void Leave(string clientId)
        {
            var idx = Users.FindIndex(x => x.Id == clientId);
            if (idx > -1)
            {
                Users.RemoveAt(idx);
                userHasChanges = true;
            }
        }

        public IEnumerable<string> Clients
        {
            get
            {
                userClients.Clear();
                foreach (var user in Users)
                {
                    userClients.Add(user.Id);
                }
                foreach (var user in Managers)
                {
                    userClients.Add(user.Id);
                }
                if (Owner != null)
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
        public IList<RoomUser> Managers { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public IList<RoomUser> Users { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }

    public class RoomUser
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}