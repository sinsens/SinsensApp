using System;
using System.Linq;
using System.Threading.Tasks;
using SinsensApp.Permissions;
using SinsensApp.Wallets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace SinsensApp.Wallets
{
    public class TransactionAppService : CrudAppService<Transaction, TransactionDto, Guid, PagedAndSortedResultRequestDto, TransactionCreateUpdateDto, TransactionCreateUpdateDto>,
        ITransactionAppService
    {
        protected override string GetPolicyName { get; set; } = SinsensAppPermissions.Transaction.Default;
        protected override string GetListPolicyName { get; set; } = SinsensAppPermissions.Transaction.Default;
        protected override string CreatePolicyName { get; set; } = SinsensAppPermissions.Transaction.Create;
        protected override string UpdatePolicyName { get; set; } = SinsensAppPermissions.Transaction.Update;
        protected override string DeletePolicyName { get; set; } = SinsensAppPermissions.Transaction.Delete;

        private readonly ITransactionRepository _repository;

        public TransactionAppService(ITransactionRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<TransactionDto> CreateAsync(TransactionCreateUpdateDto input)
        {
            var entity = MapToEntity(input);
            entity.UserId = CurrentUser.GetId();
            await Repository.InsertAsync(entity);
            return MapToGetOutputDto(entity);
        }

        protected override async Task<IQueryable<Transaction>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            query = query.Where(x => x.IsDeleted == false)
                .WhereIf(CurrentUser.Id.HasValue, x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id);
            return query;
        }
    }
}