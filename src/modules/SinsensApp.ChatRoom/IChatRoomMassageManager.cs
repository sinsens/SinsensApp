using SinsensApp.ChatRoom.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.ChatRoom
{
    public interface IChatRoomMassageManager : ISingletonDependency
    {
        string Id { get; }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        long MessageCount { get; }

        /// <summary>
        /// 加入聊天记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task AddMessageAsync(string roomId, IMessage msg);

        /// <summary>
        /// 加入聊天记录
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        Task AddMessagesAsync(string roomId, IEnumerable<IMessage> msgs);

        /// <summary>
        /// 获取全部聊天记录
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IMessage>> GetAllMessages(string roomId);

        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <param name="index">起始下标，从 0 开始</param>
        /// <param name="count">获取记录数</param>
        /// <returns></returns>
        Task<IEnumerable<IMessage>> GetMessages(string roomId, long index, long count);
    }
}