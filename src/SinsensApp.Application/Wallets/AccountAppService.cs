using Microsoft.EntityFrameworkCore;
using SinsensApp.Permissions;
using SinsensApp.Wallets.Dtos;
using SinsensApp.Wallets.Event;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Users;

namespace SinsensApp.Wallets
{
    public class AccountAppService : CrudAppService<Account, AccountDto, Guid, PagedAndSortedAccountResultRequestDto, AccountCreateUpdateDto, AccountCreateUpdateDto>,
        IAccountAppService
    {
        protected override string GetPolicyName { get; set; } = SinsensAppPermissions.Account.Default;
        protected override string GetListPolicyName { get; set; } = SinsensAppPermissions.Account.Default;
        protected override string CreatePolicyName { get; set; } = SinsensAppPermissions.Account.Create;
        protected override string UpdatePolicyName { get; set; } = SinsensAppPermissions.Account.Update;
        protected override string DeletePolicyName { get; set; } = SinsensAppPermissions.Account.Delete;

        private readonly IAccountRepository _repository;
        private readonly ILocalEventBus _eventBus;

        public AccountAppService(IAccountRepository repository,
            ILocalEventBus eventBus
            ) : base(repository)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public override async Task<AccountDto> CreateAsync(AccountCreateUpdateDto input)
        {
            var entity = MapToEntity(input);
            entity.UserId = CurrentUser.GetId();
            await Repository.InsertAsync(entity);
            await _eventBus.PublishAsync(new AccountCreatedEventEto(entity));
            return MapToGetOutputDto(entity);
        }

        public override async Task<AccountDto> UpdateAsync(Guid id, AccountCreateUpdateDto input)
        {
            var entity = await Repository.GetAsync(x => x.Id == id);
            MapToEntity(input, entity);
            await _eventBus.PublishAsync(new AccountUpdatingEventEto(entity));
            await Repository.UpdateAsync(entity);
            return MapToGetOutputDto(entity);
        }

        public override async Task<AccountDto> GetAsync(Guid id)
        {
            var query = await DefaultQuery();
            var account = await query.Where(x => x.Id == id).FirstOrDefaultAsync();
            return MapToGetOutputDto(account);
        }

        protected override async Task<IQueryable<Account>> CreateFilteredQueryAsync(PagedAndSortedAccountResultRequestDto input)
        {
            var query = await DefaultQuery();

            query = ApplyDefaultSorting(query).Where(x => x.IsDeleted == false)
                .WhereIf(CurrentUser.Id.HasValue, x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id)
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title))
                .WhereIf(input.IncludeInTotals.HasValue, x => x.IncludeInTotals == input.IncludeInTotals);

            return query.PageBy(input);
        }

        protected override IQueryable<Account> ApplyDefaultSorting(IQueryable<Account> query)
        {
            return query.IncludeDetails(true).OrderByDescending(x => x.IncludeInTotals);
        }

        private async Task<IQueryable<Account>> DefaultQuery()
        {
            var query = await Repository.GetQueryableAsync();
            return query.IncludeDetails().OrderByDescending(x => x.IncludeInTotals).ThenBy(x => x.Title);
        }
    }
}