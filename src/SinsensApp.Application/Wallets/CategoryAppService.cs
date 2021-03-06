using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SinsensApp.Permissions;
using SinsensApp.Wallets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace SinsensApp.Wallets
{
    [Authorize]
    public class CategoryAppService : CrudAppService<Category, CategoryDto, Guid, PagedAndSortedResultRequestDto, CategoryCreateUpdateDto, CategoryCreateUpdateDto>,
        ICategoryAppService
    {
        protected override string GetPolicyName { get; set; } = SinsensAppPermissions.Category.Default;
        protected override string GetListPolicyName { get; set; } = SinsensAppPermissions.Category.Default;
        protected override string CreatePolicyName { get; set; } = SinsensAppPermissions.Category.Create;
        protected override string UpdatePolicyName { get; set; } = SinsensAppPermissions.Category.Update;
        protected override string DeletePolicyName { get; set; } = SinsensAppPermissions.Category.Delete;

        private readonly ICategoryRepository _repository;

        public CategoryAppService(ICategoryRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<CategoryDto> CreateAsync(CategoryCreateUpdateDto input)
        {
            var entity = MapToEntity(input);
            entity.UserId = CurrentUser.GetId();
            await Repository.InsertAsync(entity);
            return MapToGetOutputDto(entity);
        }

        protected override async Task<IQueryable<Category>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            query = query.Where(x => x.IsDeleted == false)
                .WhereIf(CurrentUser.Id.HasValue, x => x.UserId == CurrentUser.Id || x.CreatorId == CurrentUser.Id);
            return query;
        }
    }
}