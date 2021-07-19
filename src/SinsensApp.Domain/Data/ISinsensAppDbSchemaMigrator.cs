using System.Threading.Tasks;

namespace SinsensApp.Data
{
    public interface ISinsensAppDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
