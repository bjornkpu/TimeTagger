using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Features.Login;
using Xunit.Abstractions;

namespace Api.Tests;

public class LoginTests(TestFixture fixture, ITestOutputHelper _testOutputHelper)
    : IClassFixture<TestFixture>
{
    private readonly HttpClient _httpClient = fixture.httpClient;

    [Fact]
    public async Task PostLogin_ReturnsOk()
    {
        // Arrange
        const string tag = "Work";

        // Act
        var response = await _httpClient.PostAsJsonAsync("/login", new Request
        {
            Email = "a@b.c",
            Password = "GoodPassword123"
        });

        // Assert
        var responseBody = await response.Content.ReadFromJsonAsync<Response>();
        Assert.NotNull(responseBody);
        Print(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseBody.Token);

        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(responseBody.Token);
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(jwtSecurityToken.Payload,
            new JsonSerializerOptions { WriteIndented = true }));
    }

    private async void Print<T>(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<T>();
        var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(responseBody, jsonSerializerOptions);
        _testOutputHelper.WriteLine(json);
    }

    private async void Print<T>(T responseBody)
    {
        var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(responseBody, jsonSerializerOptions);
        _testOutputHelper.WriteLine(json);
    }
}
