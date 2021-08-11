using SinsensApp.ChatRoom.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.ChatRoom
{
    public interface IChatRoomHub : IScopedDependency
    {
        Task<ResponseMessage> AddUserMsg(string roomName, string msg);

        Task<ResponseMessage> CreateRoom(string roomName);

        IEnumerable<Domain.ChatRoom> GetChatRooms();

        Task<IEnumerable<IMessage>> GetMessagesAsync();

        Task<IEnumerable<IMessage>> GetMessagesRangeAsync(int index, int count);

        Task<ResponseMessage> JoinGroup(string roomName, string userName);

        void LeaveOutRoom(string roomName);
    }
}