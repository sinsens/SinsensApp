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
        }

        public void Login(RoomUser user)
        {
            if (Users.Any(x => x.Id == user.Id) == false)
            {
                Users.Add(user);
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

        /// <summary>
        /// 房间名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 房间号
        /// </summary>
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public IList<RoomUser> Managers { get; set; }

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