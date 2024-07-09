using Api.Features.HelloWorld;


namespace Api.Tests;

public class HelloWorldTests(App App) : TestBase<App>
{

    [Fact, Priority(1)]
    public async Task GetRoot()
    {
        const string expectedResponse = "Hello World! [TimeTagger]";

        var (rsp, res) = await App.Client.GETAsync<Endpoint, Response>();

        rsp.IsSuccessStatusCode.Should().BeTrue();
        res.Message.Should().Be(expectedResponse);
    }
}
