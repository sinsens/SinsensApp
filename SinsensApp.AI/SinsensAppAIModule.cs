using System;
using Volo.Abp.AutoMapper;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace SinsensApp.AI
{
    [DependsOn(typeof(SinsensAppDomainSharedModule)
        )]
    public class SinsensAppAIModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SinsensAppAIModule>();
            });

            PreConfigure<AbpJsonOptions>(options =>
            {
                options.UseHybridSerializer = false;
            });
        }
    }
}