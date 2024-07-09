using System.Net;
using System.Net.Http.Json;
using Api.Features.Record.Post;
using Aspire.Hosting.Testing;
using Xunit.Abstractions;

namespace Api.Tests;

public class AspireTests(TestFixture fixture, ITestOutputHelper testOutputHelper)
    : IClassFixture<TestFixture>
{
    private readonly HttpClient _httpClient = fixture.httpClient;

    [Fact]
    public async Task GetApiResourceRootReturnsOkStatusCode()
    {
        // Arrange

        // Act
        var response = await _httpClient.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostRecord_WithoutTag_ReturnsOk()
    {
        // Arrange

        // Act
        var response = await _httpClient.PostAsJsonAsync("/record", new Request());

        // Assert
        Print<Response>(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostRecord_WithTag_ReturnsOk()
    {
        // Arrange
        const string tag = "Work";

        // Act
        var response = await _httpClient.PostAsJsonAsync("/record", new Request()
        {
            Tag = tag
        });

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<Response>();
        Assert.NotNull(responseBody);
        Print(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(tag, responseBody.Tag);
    }

    [Fact]
    public async Task GetRecord_ReturnsOk()
    {
        // Arrange

        // Act
        var response = await _httpClient.GetAsync("/record");

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<List<Database.Record>>();
        Assert.NotNull(responseBody);
        Print(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private async void Print<T>(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<T>();
        var jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(responseBody, jsonSerializerOptions);
        testOutputHelper.WriteLine(json);
    }
    private async void Print<T>(T responseBody)
    {
        var jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(responseBody, jsonSerializerOptions);
        testOutputHelper.WriteLine(json);
    }
}
