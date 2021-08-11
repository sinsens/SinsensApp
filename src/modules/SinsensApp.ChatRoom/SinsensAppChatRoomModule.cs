using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace SinsensApp.ChatRoom
{
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class SinsensAppChatRoomModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSignalR();
            context.Services.AddRedisMultiplexer(() =>
            {
                var connectionString = context.Services.GetConfiguration().GetConnectionString("Redis");
                return ConfigurationOptions.Parse(connectionString ?? "localhost");
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseRouting();
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatRoom.Domain.ChatRoomHub>("/hub/chatroom");
            });
        }
    }
}