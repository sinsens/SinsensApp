using System;
using System.Collections.Generic;
using System.Text;
using SinsensApp.Localization;
using Volo.Abp.Application.Services;

namespace SinsensApp
{
    /* Inherit your application services from this class.
     */

    public abstract class SinsensAppAppService : ApplicationService
    {
        protected SinsensAppAppService()
        {
            LocalizationResource = typeof(SinsensAppResource);
        }
    }
}