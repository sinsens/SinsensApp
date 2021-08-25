using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsensApp.ChatRoom
{
    public static class RedisMultiplexerConfiguration
    {
        public static IServiceCollection AddRedisMultiplexer(this IServiceCollection services, Func<ConfigurationOptions> getOptions = null)
        {
            // Get the options or assume localhost, as these will be set in
            // Startup.ConfigureServices assume they won't change
            var options = getOptions?.Invoke() ?? ConfigurationOptions.Parse("localhost");

            // The Redis is a singleton, shared as much as possible.
            return services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(options));
        }
    }
}