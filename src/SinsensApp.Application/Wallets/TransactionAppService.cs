using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using SinsensApp.Permissions;
using SinsensApp.Wallets.Dtos;
using SinsensApp.Wallets.Event;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Users;

namespace SinsensApp.Wallets
{
    [Authorize]
    public class TransactionAppService : CrudAppService<Transaction, TransactionDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateTransactionDto, CreateUpdateTransactionDto>,
        ITransactionAppService
    {
        protected override string GetPolicyName { get; set; } = SinsensAppPermissions.Transaction.Default;
        protected override string GetListPolicyName { get; set; } = SinsensAppPermissions.Transaction.Default;
        protected override string CreatePolicyName { get; set; } = SinsensAppPermissions.Transaction.Create;
        protected override string UpdatePolicyName { get; set; } = SinsensAppPermissions.Transaction.Update;
        protected override string DeletePolicyName { get; set; } = SinsensAppPermissions.Transaction.Delete;

        private readonly ILocalEventBus _localEventBus;
        private readonly ITransactionRepository _repository;
        private readonly IAccountRepository _repositoryAccount;
        private readonly IRateRepository _repositoryRate;
        private readonly ITagRepository _repositoryTag;

        public TransactionAppService(
            ILocalEventBus localEventBus,
            ITransactionRepository repository,
            IAccountRepository repositoryAccount,
            IRateRepository repositoryRate,
            ITagRepository repositoryTag) : base(repository)
        {
            _localEventBus = localEventBus;
            _repository = repository;
            _repositoryAccount = repositoryAccount;
            _repositoryRate = repositoryRate;
            _repositoryTag = repositoryTag;
        }

        public override async Task<TransactionDto> GetAsync(Guid id)
        {
            var query = await DefaultQuery();
            var entity = await query.Where(x => x.Id == id).FirstOrDefaultAsync();
            await IncludeDetails(entity);
            return MapToGetOutputDto(entity);
        }

        public override async Task<TransactionDto> UpdateAsync(Guid id, CreateUpdateTransactionDto input)
        {
            var query = await DefaultQuery();
            var entity = await query.SingleOrDefaultAsync(x => x.Id == id);

            MapToEntity(input, entity);
            if (input.TransactionType == TransactionType.Transfer && input.AccountFrom != null && input.AccountTo != null)
            {
                if (input.AccountFrom.CurrencyCode != input.AccountTo.CurrencyCode)
                {
                    var rate = await _repositoryRate.GetAsync(x => x.FromCode == input.AccountFrom.CurrencyCode && x.ToCode == input.AccountTo.CurrencyCode);
                    if (rate != null)
                    {
                        entity.ExchangeRate = rate.Ratio;
                    }
                }
            }
            await UpdateTags(input, entity);
            await Repository.UpdateAsync(entity);
            await _localEventBus.PublishAsync(new TransactionUpdatedEventEto(entity));
            await CurrentUnitOfWork.SaveChangesAsync();
            return MapToGetOutputDto(entity);
        }

        public override async Task<TransactionDto> CreateAsync(CreateUpdateTransactionDto input)
        {
            var entity = MapToEntity(input);
            entity.UserId = CurrentUser.GetId();
            if (input.TransactionType == TransactionType.Transfer && input.AccountFrom != null && input.AccountTo != null)
            {
                if (input.AccountFrom.CurrencyCode != input.AccountTo.CurrencyCode)
                {
                    var rate = await _repositoryRate.GetAsync(x => x.FromCode == input.AccountFrom.CurrencyCode && x.ToCode == input.AccountTo.CurrencyCode);
                    if (rate != null)
                    {
                        entity.ExchangeRate = rate.Ratio;
                    }
                }
            }

            await UpdateTags(input, entity);
            await _localEventBus.PublishAsync(new TransactionCreatingEventEto(entity));
            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return MapToGetOutputDto(entity);
        }

        protected override async Task<IQueryable<Transaction>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await DefaultQuery();

            query = query.Where(x => x.IsDeleted == false)
                .WhereIf(CurrentUser.Id.HasValue, x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id);
            return query;
        }

        protected override async Task<List<TransactionDto>> MapToGetListOutputDtosAsync(List<Transaction> entities)
        {
            await IncludeDetails(entities);
            return await base.MapToGetListOutputDtosAsync(entities);
        }

        protected override IQueryable<Transaction> ApplyDefaultSorting(IQueryable<Transaction> query)
        {
            return query.OrderBy(x => x.TransactionState).ThenByDescending(x => x.Date);
        }

        private async Task UpdateTags(CreateUpdateTransactionDto input, Transaction entity)
        {
            if (input.Tags.Any())
            {
                var tagIds = input.Tags.Select(x => x.Id).ToList();
                var tags = await _repositoryTag.Where(x => tagIds.Contains(x.Id)).ToArrayAsync();
                entity.Tags = tags;
            }
            else
            {
                entity.Tags.Clear();
            }
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