using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SinsensApp.Permissions;
using SinsensApp.Wallets.Dtos;
using SinsensApp.Wallets.Event;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Users;

namespace SinsensApp.Wallets
{
    public class AccountAppService : CrudAppService<Account, AccountDto, Guid, PagedAndSortedResultRequestDto, AccountCreateUpdateDto, AccountCreateUpdateDto>,
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

        public override Task<PagedResultDto<AccountDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            //_eventBus.PublishAsync(new UserRegisterEventEto { UserId = CurrentUser.GetId() });
            return base.GetListAsync(input);
        }

        public override async Task<AccountDto> CreateAsync(AccountCreateUpdateDto input)
        {
            var entity = MapToEntity(input);
            entity.UserId = CurrentUser.GetId();
            await Repository.InsertAsync(entity);
            return MapToGetOutputDto(entity);
        }

        protected override async Task<IQueryable<Account>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await Repository.GetQueryableAsync();

            query = query.Where(x => x.IsDeleted == false)
                .WhereIf(CurrentUser.Id.HasValue, x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id);
            return await base.CreateFilteredQueryAsync(input);
        }

        protected override async Task<AccountDto> MapToGetListOutputDtoAsync(Account entity)
        {
            var dto = await base.MapToGetListOutputDtoAsync(entity);

            return dto;
        }
    }
}