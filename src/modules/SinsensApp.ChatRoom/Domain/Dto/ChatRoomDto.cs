using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom.Domain.Dto
{
    public class ChatRoomDto
    {
        /// <summary>
        /// 房间名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 房间号 ID
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// 房间号
        /// </summary>
        public int Number { get; protected set; }

        /// <summary>
        /// 是否有密码
        /// </summary>
        public bool HasPassword { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        public IEnumerable<string> Clients { get; set; }
    }

    public class RoomUserDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }
    }
}