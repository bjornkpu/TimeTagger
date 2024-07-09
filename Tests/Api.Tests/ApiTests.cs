using Api.Features.Record.PostRecord;

namespace Api.Tests;

public class ApiTests(App App) : TestBase<App>
{

    [Fact, Priority(1)]
    public async Task Post()
    {
        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, Response>(new()
        {
            FirstName = "Mike",
            LastName = "Kelso",
            Age = 19,
        });

        rsp.IsSuccessStatusCode.Should().BeTrue();
        res.IsOver18.Should().Be(true);
    }
}
