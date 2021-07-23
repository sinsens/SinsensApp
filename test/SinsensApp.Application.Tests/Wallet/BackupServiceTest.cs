using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.IdentityServer.Clients;
using Xunit;

namespace SinsensApp.Wallet
{
    public class BackupServiceTest : SinsensAppApplicationTestBase
    {
        [Fact]
        public async Task RestoreFromJson()
        {
            var client = new WebClient();
            var token = await GetToken();

            client.Headers.Add("Authorization", $"Bearer {token}");
            client.Headers.Add("Content-type", "application/json; charset=utf-8");
            var result = await client.UploadFileTaskAsync($"{ApiUrl}api/app/wallet-backup/restore", "H:/temp/WalletBackupJson_2021-07-22_39fdd.json");
            var resultJson = Encoding.UTF8.GetString(result);
            Console.WriteLine(resultJson);
            result.ShouldNotBeNull();
        }
    }
}