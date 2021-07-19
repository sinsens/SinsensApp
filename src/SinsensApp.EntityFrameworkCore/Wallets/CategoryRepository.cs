using System;
using SinsensApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public class CategoryRepository : EfCoreRepository<SinsensAppDbContext, Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(IDbContextProvider<SinsensAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}