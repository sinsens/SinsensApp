using System;
using Volo.Abp.Domain.Repositories;

namespace SinsensApp.Wallets
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
    }
}