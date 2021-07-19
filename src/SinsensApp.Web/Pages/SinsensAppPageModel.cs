using SinsensApp.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace SinsensApp.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class SinsensAppPageModel : AbpPageModel
    {
        protected SinsensAppPageModel()
        {
            LocalizationResourceType = typeof(SinsensAppResource);
        }
    }
}