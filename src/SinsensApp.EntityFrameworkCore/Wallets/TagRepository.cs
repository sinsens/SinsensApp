using System;
using SinsensApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SinsensApp.Wallets
{
    public class TagRepository : EfCoreRepository<SinsensAppDbContext, Tag, Guid>, ITagRepository
    {
        public TagRepository(IDbContextProvider<SinsensAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}