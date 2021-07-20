using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
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
        private readonly IAccountRepository _repositoryAccount;

        public TransactionAppService(ITransactionRepository repository, IAccountRepository repositoryAccount) : base(repository)
        {
            _repository = repository;
            _repositoryAccount = repositoryAccount;
        }

        public override async Task<TransactionDto> GetAsync(Guid id)
        {
            var query = await DefaultQuery();
            var entity = await query.Where(x => x.Id == id).FirstOrDefaultAsync();
            await IncludeDetails(entity);
            return MapToGetOutputDto(entity);
        }

        public override async Task<TransactionDto> UpdateAsync(Guid id, TransactionCreateUpdateDto input)
        {
            var entity = MapToEntity(input);

            if (input.AccountFromId.HasValue)
            {
                entity.AccountFromId = input.AccountFromId.Value;
            }
            if (input.AccountToId.HasValue)
            {
                entity.AccountToId = input.AccountToId;
            }

            await Repository.UpdateAsync(entity);

            return MapToGetOutputDto(entity);
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
            var query = await DefaultQuery();

            query = query.Where(x => x.IsDeleted == false)
                .WhereIf(CurrentUser.Id.HasValue, x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id);
            return query.PageBy(input);
        }

        protected override async Task<List<TransactionDto>> MapToGetListOutputDtosAsync(List<Transaction> entities)
        {
            await IncludeDetails(entities);
            return await base.MapToGetListOutputDtosAsync(entities);
        }

        private async Task<List<Transaction>> IncludeDetails(List<Transaction> entities)
        {
            if (entities != null && entities.Any())
            {
                var accountFromIds = entities.Where(x => x.AccountFromId.HasValue)
                    .Select(x => x.AccountFromId).GroupBy(x => x.Value).Select(x => x.Key).ToList();
                var accountToIds = entities.Where(x => x.AccountToId.HasValue)
                    .Select(x => x.AccountToId).GroupBy(x => x.Value).Select(x => x.Key).ToList();
                var accounts = await _repositoryAccount.IncludeDetails()
                    .Where(x => accountFromIds.Contains(x.Id) || accountToIds.Contains(x.Id)).ToListAsync();

                foreach (var entity in entities)
                {
                    if (entity.AccountFromId.HasValue)
                    {
                        entity.AccountFrom = accounts.Where(x => x.Id == entity.AccountFromId.Value).FirstOrDefault();
                    }
                    if (entity.AccountToId.HasValue)
                    {
                        entity.AccountTo = accounts.Where(x => x.Id == entity.AccountToId.Value).FirstOrDefault();
                    }
                }
            }
            return entities;
        }

        private async Task<Transaction> IncludeDetails(Transaction transaction)
        {
            if (transaction != null)
            {
                if (transaction.AccountFromId.HasValue)
                {
                    transaction.AccountFrom = await _repositoryAccount.IncludeDetails()
                        .Where(x => x.Id == transaction.AccountFromId.Value).FirstOrDefaultAsync();
                }
                if (transaction.AccountToId.HasValue)
                {
                    transaction.AccountTo = await _repositoryAccount.IncludeDetails()
                        .Where(x => x.Id == transaction.AccountToId.Value).FirstOrDefaultAsync();
                }
            }
            return transaction;
        }

        private async Task<IQueryable<Transaction>> DefaultQuery()
        {
            var query = await Repository.GetQueryableAsync();
            return query.IncludeDetails();
        }
    }
}