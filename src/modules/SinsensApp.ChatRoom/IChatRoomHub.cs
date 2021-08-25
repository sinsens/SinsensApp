using SinsensApp.ChatRoom.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.ChatRoom
{
    public interface IChatRoomHub : IScopedDependency
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="roomName">房间名称</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<ResponseMessage> AddUserMsgAsync(string roomName, string msg);

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="roomName">房间名称</param>
        /// <param name="password">房间密码</param>
        /// <returns></returns>
        Task<ResponseMessage> CreateRoomAsync(string roomName, string password = null);

        /// <summary>
        /// 获取房间列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Domain.Dto.ChatRoomDto>> GetChatRoomsAsync();

        /// <summary>
        /// 获取房间所有消息
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        Task<IEnumerable<IMessage>> GetMessagesAsync(string roomName);

        /// <summary>
        /// 按批次获取房间消息
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<IEnumerable<IMessage>> GetMessagesRangeAsync(string roomName, int index, int count);

        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="password">房间密码</param>
        /// <returns></returns>
        Task<ResponseMessage> JoinRoomAsync(string roomName, string password = null);

        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="roomName"></param>
        Task LeaveOutRoomAsync(string roomName);

        /// <summary>
        /// 断线重连
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task ReConnectedAsync(string clientId);

        Task<ResponseMessage> UpdateMyNameAsync(string nickName);
    }
}