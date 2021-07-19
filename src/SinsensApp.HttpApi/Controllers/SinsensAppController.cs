using SinsensApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SinsensApp.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class SinsensAppController : AbpController
    {
        protected SinsensAppController()
        {
            LocalizationResource = typeof(SinsensAppResource);
        }
    }
}