using Microsoft.EntityFrameworkCore;
using SinsensApp.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;
using SinsensApp.Wallets;

namespace SinsensApp.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See SinsensAppMigrationsDbContext for migrations.
     */

    [ConnectionStringName("Default")]
    public class SinsensAppDbContext : AbpDbContext<SinsensAppDbContext>
    {
        public DbSet<AppUser> Users { get; set; }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside SinsensAppDbContextModelCreatingExtensions.ConfigureSinsensApp
         */
        /*
         public DbSet<Transaction> Transactions { get; set; }
         public DbSet<Account> Accounts { get; set; }
         public DbSet<Tag> Tags { get; set; }
         public DbSet<Category> Categories { get; set; }
         public DbSet<TransactionAttachment> TransactionAttachments { get; set; }
        */

        public SinsensAppDbContext(DbContextOptions<SinsensAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                /* Configure mappings for your additional properties
                 * Also see the SinsensAppEfCoreEntityExtensionMappings class
                 */
            });

            /* Configure your own tables/entities inside the ConfigureSinsensApp method */

            builder.ConfigureSinsensApp();
        }
    }
}