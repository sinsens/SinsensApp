using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Identity;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.PermissionManagement;
using SinsensApp.Permissions;
using SinsensApp.Wallets;
using Volo.Abp.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Volo.Abp.Json;
using Volo.Abp.Domain.Entities.Events;

namespace SinsensApp.Data
{
    public class WalletDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        public ILogger<WalletDataSeedContributor> Logger { get; set; }

        private const string WalletUserRoleName = "WalletUser";

        private readonly ITenantRepository _tenantRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IRepository<IdentityRole> _identityRoles;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IJsonSerializer _jsonSerilazer;

        public WalletDataSeedContributor(
            ITenantRepository tenantRepository,
            ICurrentTenant currentTenant,
            IRepository<IdentityRole> identityRoles,
            IRepository<Currency> currencyRepository,
            IPermissionManager permissionManager,
            IGuidGenerator guidGenerator,
            IJsonSerializer jsonSerilazer
            )
        {
            _tenantRepository = tenantRepository;
            _currentTenant = currentTenant;
            _identityRoles = identityRoles;
            _currencyRepository = currencyRepository;
            _permissionManager = permissionManager;
            _guidGenerator = guidGenerator;
            _jsonSerilazer = jsonSerilazer;
            Logger = NullLogger<WalletDataSeedContributor>.Instance;
        }

        private async Task<bool> AddInitialMigrationIfNotExist()
        {
            return await _identityRoles.AnyAsync(x => x.Name == WalletUserRoleName) && await _currencyRepository.AnyAsync(x => x.Code != null);
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var initialMigrationAdded = await AddInitialMigrationIfNotExist();

            if (initialMigrationAdded)
            {
                return;
            }

            await SeedDataAsync();
            await SeedCurrencyDataAsync();

            var tenants = await _tenantRepository.GetListAsync(includeDetails: true);
            foreach (var tenant in tenants)
            {
                using (_currentTenant.Change(tenant.Id))
                {
                    await SeedDataAsync(tenant);
                }
            }
        }

        private async Task SeedDataAsync(Tenant tenant = null)
        {
            await SeedPermissionDataAsync(tenant);
        }

        private async Task SeedCurrencyDataAsync()
        {
            Volo.Abp.EventBus.Local.NullLocalEventBus.Instance.UnsubscribeAll(typeof(EntityCreatedEventData<Account>));
            Volo.Abp.EventBus.Local.NullLocalEventBus.Instance.UnsubscribeAll(typeof(EntityUpdatingEventData<Account>));
            var currenciesInDb = await _currencyRepository.GetListAsync();
            if (System.IO.File.Exists("Currencies.json"))
            {
                Logger.LogInformation("File Currencies.json exist, loading。");
                var content = System.IO.File.ReadAllText("Currencies.json");
                var currencies = _jsonSerilazer.Deserialize<IEnumerable<Currency>>(content);
                var tempList = new HashSet<string>();
                if (currencies != null && currencies.Any())
                {
                    foreach (var currency in currencies)
                    {
                        if (currenciesInDb.Any(x => x.Code == currency.Code) == false && tempList.Contains(currency.Code) == false)
                        {
                            await _currencyRepository.InsertAsync(currency);
                            tempList.Add(currency.Code);
                        }
                    }
                }
            }
        }

        #region 权限

        private async Task SeedPermissionDataAsync(Tenant tenant = null)
        {
            IdentityRole role;
            if (tenant == null)
            {
                role = await _identityRoles.FirstOrDefaultAsync(x => x.Name == WalletUserRoleName && x.TenantId == null);
            }
            else
            {
                role = await _identityRoles.FirstOrDefaultAsync(x => x.Name == WalletUserRoleName && x.TenantId == tenant.Id);
            }
            if (role == null)
            {
                role = new IdentityRole(_guidGenerator.Create(), WalletUserRoleName, tenant?.Id);
                role.IsDefault = true;
                role.IsPublic = true;
                role.IsStatic = true;
                await _identityRoles.InsertAsync(role);
            }
            var walletPermissions = new List<string> {
                SinsensAppPermissions.Account.Default,
                SinsensAppPermissions.Account.Create,
                SinsensAppPermissions.Account.Update,
                SinsensAppPermissions.Account.Delete,

                SinsensAppPermissions.Category.Default,
                SinsensAppPermissions.Category.Create,
                SinsensAppPermissions.Category.Update,
                SinsensAppPermissions.Category.Delete,

                SinsensAppPermissions.Tag.Default,
                SinsensAppPermissions.Tag.Create,
                SinsensAppPermissions.Tag.Update,
                SinsensAppPermissions.Tag.Delete,

                SinsensAppPermissions.Transaction.Default,
                SinsensAppPermissions.Transaction.Create,
                SinsensAppPermissions.Transaction.Update,
                SinsensAppPermissions.Transaction.Delete,

                SinsensAppPermissions.TransactionAttachment.Default,
                SinsensAppPermissions.TransactionAttachment.Create,
                SinsensAppPermissions.TransactionAttachment.Update,
                SinsensAppPermissions.TransactionAttachment.Delete,
            };

            foreach (var permission in walletPermissions)
            {
                await _permissionManager.SetForRoleAsync(WalletUserRoleName, permission, true);
            }
        }

        #endregion 权限
    }
}