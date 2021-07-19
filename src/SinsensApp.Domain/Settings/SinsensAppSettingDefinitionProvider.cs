using Volo.Abp.Settings;

namespace SinsensApp.Settings
{
    public class SinsensAppSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(SinsensAppSettings.MySetting1));
        }
    }
}
