using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.Web
{
    [Dependency(ReplaceServices = true)]
    public class SinsensAppBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "SinsensApp";
    }
}
