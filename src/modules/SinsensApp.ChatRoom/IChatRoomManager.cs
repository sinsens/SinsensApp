using SinsensApp.ChatRoom.Domain;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.ChatRoom
{
    public interface IChatRoomManager : ISingletonDependency
    {
        IList<Domain.ChatRoom> Rooms { get; }

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="roomName">房间名称</param>
        /// <param name="ownerId"></param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        (bool isOk, string message) CreateRoom(string roomName, string ownerId, string password = null);

        /// <summary>
        /// 查找用户是否存在于指定房间
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        RoomUser FindUser(string roomName, string clientId);

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        RoomUser FindUser(string clientId);

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        Domain.ChatRoom GetChatRoom(string roomName);

        /// <summary>
        /// 是否加入该房间
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        bool HasJoinRoom(string roomName, string clientId);

        /// <summary>
        /// 是否存在该房间
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        bool IsExist(string roomName);

        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="roomName">房间名</param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        (bool isOk, string message) JoinRoom(string roomName, string clientId, string password = null);

        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        bool LeaveRoom(string roomName, string clientId);

        RoomUser Login(string clientId, string nickName);

        void ReConnect(string oldClientId, string newClientId);

        void SaveData();
    }
}