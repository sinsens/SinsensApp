using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.ChatRoom
{
    public interface IChatRoomManager : ISingletonDependency
    {
        IList<Domain.ChatRoom> Rooms { get; }

        (bool isOk, string message) CreateRoom(string roomName, string ownerId);

        bool HasJoinRoom(string roomName, string clientId);

        bool IsExist(string roomName);

        (bool isOk, string message) JoinRoom(string roomName, string clientId, string name);

        bool LeaveRoom(string roomName, string clientId);
    }
}