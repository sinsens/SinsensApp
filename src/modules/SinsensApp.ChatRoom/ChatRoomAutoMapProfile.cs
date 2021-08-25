using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom
{
    public class ChatRoomAutoMapProfile : Profile
    {
        public ChatRoomAutoMapProfile()
        {
            CreateMap<Domain.RoomUser, Domain.Dto.RoomUserDto>();
            CreateMap<ChatRoom.Domain.ChatRoom, ChatRoom.Domain.Dto.ChatRoomDto>();
        }
    }
}