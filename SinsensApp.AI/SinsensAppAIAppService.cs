using SinsensApp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SinsensApp.AI
{
    public class SinsensAppAIAppService : ApplicationService
    {
        protected SinsensAppAIAppService()
        {
            LocalizationResource = typeof(SinsensAppResource);
        }
    }
}