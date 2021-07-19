using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SinsensApp.Data
{
    /* This is used if database provider does't define
     * ISinsensAppDbSchemaMigrator implementation.
     */
    public class NullSinsensAppDbSchemaMigrator : ISinsensAppDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}