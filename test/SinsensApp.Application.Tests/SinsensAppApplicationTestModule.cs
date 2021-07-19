using Volo.Abp.Modularity;

namespace SinsensApp
{
    [DependsOn(
        typeof(SinsensAppApplicationModule),
        typeof(SinsensAppDomainTestModule)
        )]
    public class SinsensAppApplicationTestModule : AbpModule
    {

    }
}