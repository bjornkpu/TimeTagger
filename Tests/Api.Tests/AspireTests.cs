using System.Net;
using System.Net.Http.Json;
using Aspire.Hosting.Testing;
using Xunit.Abstractions;
using PostRequest = Api.Features.Record.Post.Request;
using PostResponse = Api.Features.Record.Post.Response;
using PatchRequest = Api.Features.Record.Update.Request;
using PatchResponse = Api.Features.Record.Update.Response;

namespace Api.Tests;

public class AspireTests(TestFixture fixture, ITestOutputHelper _testOutputHelper)
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
        var response = await _httpClient.PostAsJsonAsync("/record", new PostRequest());

        // Assert
        Print<PostResponse>(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostRecord_WithTag_ReturnsOk()
    {
        // Arrange
        const string tag = "Work";

        // Act
        var response = await _httpClient.PostAsJsonAsync("/record", new PostRequest()
        {
            Tag = tag
        });

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<PostResponse>();
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

    [Fact]
    public async Task DeleteRecord_ReturnsNoContent()
    {
        // Arrange
        var httpResponse = await _httpClient.PostAsJsonAsync("/record", new PostRequest());
        var record = await httpResponse.Content.ReadFromJsonAsync<Database.Record>();
        Assert.NotNull(record);

        // Act
        var response = await _httpClient.DeleteAsync($"/record/{record.Id}");

        // Assert
        _testOutputHelper.WriteLine(response.ToString());
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseBody);
        Print(responseBody);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateRecord_WrongId_ReturnsNoContent()
    {
        // Arrange
        var guid = new Guid().ToString();

        // Act
        var response = await _httpClient.PatchAsJsonAsync($"/record/{guid}", new PatchRequest());

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateRecord_Tag_ReturnsOk()
    {
        // Arrange
        var httpResponse = await _httpClient.PostAsJsonAsync("/record", new PostRequest());
        var record = await httpResponse.Content.ReadFromJsonAsync<PostResponse>();
        Assert.NotNull(record);
        Assert.Null(record.Tag);

        const string tag = "NewTag";

        // Act
        var response = await _httpClient.PatchAsJsonAsync($"/record/{record.Id}", new PatchRequest()
        {
            Tag = tag
        });

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<PatchResponse>();
        Assert.NotNull(responseBody);
        Print(responseBody);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(tag, responseBody.Tag);
    }

    [Fact]
    public async Task UpdateRecord_Timestamp_ReturnsOk()
    {
        // Arrange
        var httpResponse = await _httpClient.PostAsJsonAsync("/record", new PostRequest());
        var record = await httpResponse.Content.ReadFromJsonAsync<PostResponse>();
        Assert.NotNull(record);
        Print(record);

        var oldTime = record.Timestamp;
        var newTime = DateTime.UtcNow;

        // Act
        await Task.Delay(2);
        var response = await _httpClient.PatchAsJsonAsync($"/record/{record.Id}", new PatchRequest()
        {
            Timestamp = newTime
        });

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<PatchResponse>();
        Assert.NotNull(responseBody);
        Print(responseBody);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEqual(oldTime, responseBody.Timestamp);
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
