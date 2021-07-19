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
    public class TagAppService : CrudAppService<Tag, TagDto, Guid, PagedAndSortedResultRequestDto, TagCreateUpdateDto, TagCreateUpdateDto>,
        ITagAppService
    {
        protected override string GetPolicyName { get; set; } = SinsensAppPermissions.Tag.Default;
        protected override string GetListPolicyName { get; set; } = SinsensAppPermissions.Tag.Default;
        protected override string CreatePolicyName { get; set; } = SinsensAppPermissions.Tag.Create;
        protected override string UpdatePolicyName { get; set; } = SinsensAppPermissions.Tag.Update;
        protected override string DeletePolicyName { get; set; } = SinsensAppPermissions.Tag.Delete;

        private readonly ITagRepository _repository;

        public TagAppService(ITagRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<TagDto> CreateAsync(TagCreateUpdateDto input)
        {
            var entity = MapToEntity(input);
            entity.UserId = CurrentUser.GetId();
            await Repository.InsertAsync(entity);
            return MapToGetOutputDto(entity);
        }

        protected override async Task<IQueryable<Tag>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            query = query.Where(x => x.IsDeleted == false)
                .WhereIf(CurrentUser.Id.HasValue, x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id);
            return query;
        }
    }
}