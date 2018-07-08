using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil.Messages;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Mediator.Net.WebApiSample.Test
{
    public class EndpointsTest: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public EndpointsTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/values/how are you")]
        public async Task RequestResponseTest(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var simpleResponse = JsonConvert.DeserializeObject<SimpleResponse>(result);
            simpleResponse.EchoMessage.ShouldBe("how are you");
        }
    }
}
