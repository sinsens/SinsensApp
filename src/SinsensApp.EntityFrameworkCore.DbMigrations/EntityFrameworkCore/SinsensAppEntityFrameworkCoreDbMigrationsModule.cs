using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace SinsensApp.EntityFrameworkCore
{
    [DependsOn(
        typeof(SinsensAppEntityFrameworkCoreModule)
        )]
    public class SinsensAppEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SinsensAppMigrationsDbContext>();
        }
    }
}
