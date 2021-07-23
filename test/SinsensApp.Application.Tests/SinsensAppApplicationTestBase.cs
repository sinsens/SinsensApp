using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.IdentityServer;
using Xunit;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace SinsensApp
{
    public abstract class SinsensAppApplicationTestBase : SinsensAppTestBase<SinsensAppApplicationTestModule>
    {
        public string Token { get; set; } = string.Empty;
        public const string ApiUrl = "https://localhost:44321/";

        [Fact]
        public virtual async Task<string> GetToken()
        {
            if (Token.IsNullOrWhiteSpace())
            {
                var client = new HttpClient();

                var ids4info = await client.GetStringAsync($"{ApiUrl}.well-known/openid-configuration");
                var serverinfo = JsonConvert.DeserializeObject<JObject>(ids4info);
                var clientSetting = new
                {
                    grant_type = "password",
                    scope = "SinsensApp",
                    username = "admin",
                    password = "1q2w3E*",
                    client_id = "SinsensApp_App",
                    client_secret = "1q2w3e*"
                };

                var uri = new Uri(serverinfo["token_endpoint"].Value<string>());
                //client.DefaultRequestHeaders.Add("content-type", "application/x-www-form-urlencoded");

                var list = new List<KeyValuePair<String, String>>();
                list.Add(new KeyValuePair<string, string>("grant_type", clientSetting.grant_type));
                list.Add(new KeyValuePair<string, string>("scope", clientSetting.scope));
                list.Add(new KeyValuePair<string, string>("client_id", clientSetting.client_id));
                list.Add(new KeyValuePair<string, string>("client_secret", clientSetting.client_secret));
                list.Add(new KeyValuePair<string, string>("username", clientSetting.username));
                list.Add(new KeyValuePair<string, string>("password", clientSetting.password));

                var httpcontent = new FormUrlEncodedContent(list);

                var result = await client.PostAsync(uri, httpcontent);

                var content = await result.Content.ReadAsStringAsync();

                var token_info = JsonConvert.DeserializeObject<JObject>(content);

                Token = token_info["access_token"].Value<string>();
            }
            Assert.NotEmpty(Token);
            return Token;
        }
    }
}