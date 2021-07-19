using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SinsensApp.EntityFrameworkCore;
using SinsensApp.Wallets;
using System.Collections.Generic;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace SinsensApp.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(SinsensAppEntityFrameworkCoreDbMigrationsModule),
        typeof(SinsensAppApplicationContractsModule)
        )]
    public class SinsensAppDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}