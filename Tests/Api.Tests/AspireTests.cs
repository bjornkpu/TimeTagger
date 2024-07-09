using System.Net;
using System.Net.Http.Json;
using Api.Features.Record.Post;
using Aspire.Hosting.Testing;
using Xunit.Abstractions;

namespace Api.Tests;

public class AspireTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AspireTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetApiResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("api");
        var response = await httpClient.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostRecord_WithoutTag_ReturnsOk()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("api");
        var response = await httpClient.PostAsJsonAsync("/record", new Request());

        Print<Response>(response);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostRecord_WithTag_ReturnsOk()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        var tag = "Work";

        // Act
        var httpClient = app.CreateHttpClient("api");
        var response = await httpClient.PostAsJsonAsync("/record", new Request()
        {
            Tag = tag
        });


        var responseBody = await response.Content.ReadFromJsonAsync<Response>();
        Assert.NotNull(responseBody);

        Print(responseBody);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(tag, responseBody.Tag);
    }

    private async void Print<T>(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<T>();
        var jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(responseBody, jsonSerializerOptions);
        _testOutputHelper.WriteLine(json);
    }
    private async void Print<T>(T responseBody)
    {
        var jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(responseBody, jsonSerializerOptions);
        _testOutputHelper.WriteLine(json);
    }
}
