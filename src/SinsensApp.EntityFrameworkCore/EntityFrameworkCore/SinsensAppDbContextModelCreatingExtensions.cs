using SinsensApp.Wallets;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System;

namespace SinsensApp.EntityFrameworkCore
{
    public static class SinsensAppDbContextModelCreatingExtensions
    {
        public static void ConfigureSinsensApp(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(SinsensAppConsts.DbTablePrefix + "YourEntities", SinsensAppConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            builder.Entity<Transaction>(b =>
            {
                b.Ignore(c => c.AccountFrom).Ignore(c => c.AccountTo);
                b.Property(c => c.AccountToId).IsRequired(false);
                b.Property(c => c.AccountFromId).IsRequired(false);

                b.ToTable(SinsensAppConsts.WalletDbTablePrefix + "Transactions", SinsensAppConsts.DbSchema);
                b.ConfigureByConvention();

                /* Configure more properties here */
            });

            builder.Entity<Account>(b =>
            {
                b.ToTable(SinsensAppConsts.WalletDbTablePrefix + "Accounts", SinsensAppConsts.DbSchema);
                b.ConfigureByConvention();
                /* Configure more properties here */
            });

            builder.Entity<Tag>(b =>
            {
                b.ToTable(SinsensAppConsts.WalletDbTablePrefix + "Tags", SinsensAppConsts.DbSchema);
                b.ConfigureByConvention();

                /* Configure more properties here */
            });

            builder.Entity<Category>(b =>
            {
                b.ToTable(SinsensAppConsts.WalletDbTablePrefix + "Categories", SinsensAppConsts.DbSchema);
                b.ConfigureByConvention();

                /* Configure more properties here */
            });

            builder.Entity<TransactionAttachment>(b =>
            {
                b.ToTable(SinsensAppConsts.WalletDbTablePrefix + "TransactionAttachments", SinsensAppConsts.DbSchema);
                b.ConfigureByConvention();

                /* Configure more properties here */
            });

            builder.Entity<Currency>(b =>
            {
                b.ToTable(SinsensAppConsts.WalletDbTablePrefix + "Currencies", SinsensAppConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<CurrencyRate>(b =>
            {
                b.ToTable(SinsensAppConsts.WalletDbTablePrefix + "CurrencyRates", SinsensAppConsts.DbSchema);
                b.ConfigureByConvention();
            });
        }
    }
}