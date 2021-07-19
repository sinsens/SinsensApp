using System;
using System.Threading.Tasks;
using SinsensApp.Wallets;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace SinsensApp.EntityFrameworkCore.Wallets
{
    public class TransactionRepositoryTests : SinsensAppEntityFrameworkCoreTestBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionRepositoryTests()
        {
            _transactionRepository = GetRequiredService<ITransactionRepository>();
        }

        /*
        [Fact]
        public async Task Test1()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                // Arrange

                // Act

                //Assert
            });
        }
        */
    }
}
