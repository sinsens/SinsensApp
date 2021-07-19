using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SinsensApp.Permissions;
using SinsensApp.Wallets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace SinsensApp.Wallets
{
    public class TransactionAttachmentAppService : CrudAppService<TransactionAttachment, TransactionAttachmentDto, Guid, PagedAndSortedResultRequestDto, TransactionAttachmentCreateUpdateDto, TransactionAttachmentCreateUpdateDto>,
        ITransactionAttachmentAppService
    {
        protected override string GetPolicyName { get; set; } = SinsensAppPermissions.TransactionAttachment.Default;
        protected override string GetListPolicyName { get; set; } = SinsensAppPermissions.TransactionAttachment.Default;
        protected override string CreatePolicyName { get; set; } = SinsensAppPermissions.TransactionAttachment.Create;
        protected override string UpdatePolicyName { get; set; } = SinsensAppPermissions.TransactionAttachment.Update;
        protected override string DeletePolicyName { get; set; } = SinsensAppPermissions.TransactionAttachment.Delete;

        private readonly ITransactionAttachmentRepository _repository;

        public TransactionAttachmentAppService(ITransactionAttachmentRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}