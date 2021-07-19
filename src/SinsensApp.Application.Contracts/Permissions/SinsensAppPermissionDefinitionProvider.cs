using SinsensApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SinsensApp.Permissions
{
    public class SinsensAppPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(SinsensAppPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(SinsensAppPermissions.MyPermission1, L("Permission:MyPermission1"));

            var transactionPermission = myGroup.AddPermission(SinsensAppPermissions.Transaction.Default, L("Permission:Transaction"));
            transactionPermission.AddChild(SinsensAppPermissions.Transaction.Create, L("Permission:Create"));
            transactionPermission.AddChild(SinsensAppPermissions.Transaction.Update, L("Permission:Update"));
            transactionPermission.AddChild(SinsensAppPermissions.Transaction.Delete, L("Permission:Delete"));

            var accountPermission = myGroup.AddPermission(SinsensAppPermissions.Account.Default, L("Permission:Account"));
            accountPermission.AddChild(SinsensAppPermissions.Account.Create, L("Permission:Create"));
            accountPermission.AddChild(SinsensAppPermissions.Account.Update, L("Permission:Update"));
            accountPermission.AddChild(SinsensAppPermissions.Account.Delete, L("Permission:Delete"));

            var tagPermission = myGroup.AddPermission(SinsensAppPermissions.Tag.Default, L("Permission:Tag"));
            tagPermission.AddChild(SinsensAppPermissions.Tag.Create, L("Permission:Create"));
            tagPermission.AddChild(SinsensAppPermissions.Tag.Update, L("Permission:Update"));
            tagPermission.AddChild(SinsensAppPermissions.Tag.Delete, L("Permission:Delete"));

            var categoryPermission = myGroup.AddPermission(SinsensAppPermissions.Category.Default, L("Permission:Category"));
            categoryPermission.AddChild(SinsensAppPermissions.Category.Create, L("Permission:Create"));
            categoryPermission.AddChild(SinsensAppPermissions.Category.Update, L("Permission:Update"));
            categoryPermission.AddChild(SinsensAppPermissions.Category.Delete, L("Permission:Delete"));

            var transactionAttachmentPermission = myGroup.AddPermission(SinsensAppPermissions.TransactionAttachment.Default, L("Permission:TransactionAttachment"));
            transactionAttachmentPermission.AddChild(SinsensAppPermissions.TransactionAttachment.Create, L("Permission:Create"));
            transactionAttachmentPermission.AddChild(SinsensAppPermissions.TransactionAttachment.Update, L("Permission:Update"));
            transactionAttachmentPermission.AddChild(SinsensAppPermissions.TransactionAttachment.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SinsensAppResource>(name);
        }
    }
}
