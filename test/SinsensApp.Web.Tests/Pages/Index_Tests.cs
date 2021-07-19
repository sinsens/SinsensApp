using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace SinsensApp.Pages
{
    public class Index_Tests : SinsensAppWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
