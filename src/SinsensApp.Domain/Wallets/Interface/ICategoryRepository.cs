using System;
using Volo.Abp.Domain.Repositories;

namespace SinsensApp.Wallets
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
    }
}