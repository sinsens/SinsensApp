using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AutoMapper;
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
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SinsensAppChatRoomModule>();
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

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var roomManager = context.ServiceProvider.GetService<IChatRoomManager>();
            if (roomManager != null)
            {
                roomManager.SaveData();
            }
            base.OnApplicationShutdown(context);
        }
    }
}