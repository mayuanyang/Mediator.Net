using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mediator.Net.TestUtil.Messages;
using Mediator.Net.WebApiSample.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Mediator.Net.WebApiSample.Test;

public class EndpointsTest: IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public EndpointsTest(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        
        Recorder.Values.Clear();
    }

    [Theory]
    [InlineData("/api/values/a request response message: how are you")]
    public async Task CanSendRequestResponse(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadAsStringAsync();
        var simpleResponse = JsonConvert.DeserializeObject<SimpleResponse>(result);
        
        simpleResponse.EchoMessage.ShouldBe("a request response message: how are you");
    }

    [Theory]
    [InlineData("/api/values/command")]
    public async Task CanSendCommand(string url)
    {
        var client = _factory.CreateClient();
        var commandData = new CommandData() {Left = 1, Right = 2};
        var content = new StringContent(JsonConvert.SerializeObject(commandData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        
        response.EnsureSuccessStatusCode();
        
        Recorder.Values.Count.ShouldBe(3);
        Recorder.Values.Single(x => x.ToString() == commandData.Left.ToString());
        Recorder.Values.Single(x => x.ToString() == commandData.Right.ToString());
        Recorder.Values.Single(x => x.ToString() == (commandData.Left + commandData.Right).ToString());
    }

    [Theory]
    [InlineData("/api/values/event")]
    public async Task CanPublishEvent(string url)
    {
        var client = _factory.CreateClient();
        var eventData = new EventData(){Result = 100};
        var content = new StringContent(JsonConvert.SerializeObject(eventData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        
        response.EnsureSuccessStatusCode();
        
        Recorder.Values.Count.ShouldBe(1);
        Recorder.Values.Single(x => x.ToString() == eventData.Result.ToString());
    }
}