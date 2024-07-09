using Aspire.Hosting.Testing;

namespace Api.Tests;

public class TestFixture : IAsyncLifetime
{
    private DistributedApplication app;
    public HttpClient httpClient;

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        app = await appHost.BuildAsync();
        await app.StartAsync();
        httpClient = app.CreateHttpClient("api");
    }

    public async Task DisposeAsync()
    {
        httpClient.Dispose();
        await app.DisposeAsync();
    }
}
