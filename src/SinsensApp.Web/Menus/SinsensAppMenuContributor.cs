using System.Threading.Tasks;
using SinsensApp.Permissions;
using SinsensApp.Localization;
using SinsensApp.MultiTenancy;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace SinsensApp.Web.Menus
{
    public class SinsensAppMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<SinsensAppResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    SinsensAppMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "fas fa-home",
                    order: 0
                )
            );
            
            if (MultiTenancyConsts.IsEnabled)
            {
                administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
            }
            else
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
            if (await context.IsGrantedAsync(SinsensAppPermissions.Account.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(SinsensAppMenus.Account, l["Menu:Account"], "/Wallets/Account")
                );
            }
            if (await context.IsGrantedAsync(SinsensAppPermissions.Tag.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(SinsensAppMenus.Tag, l["Menu:Tag"], "/Wallets/Tag")
                );
            }
            if (await context.IsGrantedAsync(SinsensAppPermissions.Category.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(SinsensAppMenus.Category, l["Menu:Category"], "/Wallets/Category")
                );
            }
            if (await context.IsGrantedAsync(SinsensAppPermissions.Transaction.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(SinsensAppMenus.Transaction, l["Menu:Transaction"], "/Wallets/Transaction")
                );
            }
            if (await context.IsGrantedAsync(SinsensAppPermissions.TransactionAttachment.Default))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(SinsensAppMenus.TransactionAttachment, l["Menu:TransactionAttachment"], "/Wallets/TransactionAttachment")
                );
            }
        }
    }
}
