using Microsoft.Extensions.DependencyInjection;
using SinsensApp.Wallets;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace SinsensApp
{
    [DependsOn(
        typeof(SinsensAppDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(SinsensAppApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
        )]
    public class SinsensAppApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SinsensAppApplicationModule>();
            });
            Configure<AbpDataFilterOptions>(options =>
            {
                options.DefaultStates[typeof(ISoftDelete)] = new DataFilterState(isEnabled: true);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
            context.AddBackgroundWorker<AutoSyncCurrencyRates>();
        }
    }
}