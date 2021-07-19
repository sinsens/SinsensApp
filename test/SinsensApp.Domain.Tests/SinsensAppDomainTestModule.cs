using SinsensApp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SinsensApp
{
    [DependsOn(
        typeof(SinsensAppEntityFrameworkCoreTestModule)
        )]
    public class SinsensAppDomainTestModule : AbpModule
    {

    }
}