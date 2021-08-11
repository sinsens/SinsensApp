using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace SinsensApp.ChatRoom.Domain
{
    public class ChatRoomMassageManager : IChatRoomMassageManager
    {
        public string Id { get; set; }

        public long MessageCount
        {
            get
            {
                if (_database == null || _database.IsConnected(Id) == false)
                {
                    return 0;
                }
                return _database.ListLength(Id);
            }
        }

        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly IJsonSerializer _serializer;

        public ChatRoomMassageManager(IConnectionMultiplexer connection, IJsonSerializer serializer)
        {
            _serializer = serializer;
            _connectionMultiplexer = connection;
            _database = _connectionMultiplexer.GetDatabase(0);
        }

        public async Task AddMessageAsync(IMessage msg)
        {
            await _database.ListRightPushAsync(Id, _serializer.Serialize(msg));
        }

        public async Task<IEnumerable<IMessage>> GetAllMessages()
        {
            var list = await _database.ListRangeAsync(Id);
            var result = list.Select(x => _serializer.Deserialize<IMessage>(x));
            return result;
        }

        public async Task<IEnumerable<IMessage>> GetMessages(long index, long count = 10)
        {
            var stop = count < 2 ? 1 : count - 1;
            var list = await _database.ListRangeAsync(Id, index, index + stop);
            var result = list.Select(x => _serializer.Deserialize<IMessage>(x));
            return result;
        }

        public async Task AddMessagesAsync(IEnumerable<IMessage> msgs)
        {
            foreach (var msg in msgs)
            {
                await _database.ListRightPushAsync(Id, _serializer.Serialize(msg));
            }
        }
    }
}