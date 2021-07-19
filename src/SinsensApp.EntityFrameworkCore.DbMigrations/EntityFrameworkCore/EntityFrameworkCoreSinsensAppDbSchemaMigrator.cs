using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SinsensApp.Data;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.EntityFrameworkCore
{
    public class EntityFrameworkCoreSinsensAppDbSchemaMigrator
        : ISinsensAppDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreSinsensAppDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the SinsensAppMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<SinsensAppMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}