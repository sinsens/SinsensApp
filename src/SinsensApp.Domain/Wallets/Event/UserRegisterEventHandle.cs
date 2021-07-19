using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using SinsensApp.Localization;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Timing;
using Volo.Abp.Domain.Entities.Events;
using SinsensApp.Users;
using Volo.Abp.Data;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace SinsensApp.Wallets.Event
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class UserRegisterEventHandle : DomainService, ILocalEventHandler<EntityCreatedEventData<IdentityUser>>, ITransientDependency
    {
        private readonly IdentityUserManager _identityUserManager;
        private readonly IStringLocalizer<SinsensAppResource> _localizer;
        private readonly IAccountRepository _repository;
        private readonly ICurrencyRepository _currencies;

        public UserRegisterEventHandle(
            IdentityUserManager identityUserManager,
            IStringLocalizer<SinsensAppResource> localizer,
            IAccountRepository repository,
            ICurrencyRepository currencies)
        {
            _identityUserManager = identityUserManager;
            _localizer = localizer;
            _repository = repository;
            _currencies = currencies;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            var userId = eventData.Entity.Id;
            var account = await _repository.FirstOrDefaultAsync(x => x.UserId == userId);
            var defaultCuurency = await _currencies.FirstOrDefaultAsync(x => x.Code == WalletsConsts.DefaultCurrencyCode);
            if (account == null && defaultCuurency != null)
            {
                await _repository.InsertAsync(new Account
                {
                    CurrencyCode = defaultCuurency.Code,
                    Title = L("默认钱包"),
                    UserId = userId,
                    IncludeInTotals = true,
                    Currency = defaultCuurency
                });
                // 授权
                var user = await _identityUserManager.GetByIdAsync(userId);
                await _identityUserManager.SetRolesAsync(user, new string[] { "WalletUser" });
            }
        }

        private string L(string name)
        {
            return _localizer.GetString(name);
        }
    }
}