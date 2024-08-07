using FastEndpoints;
using FastEndpoints.Security;

namespace Api.Features.Login;

public class UserLoginEndpoint : Endpoint<Request,Response>
{
    public override void Configure()
    {
        Post("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (!await AuthService.CredentialsAreValid(req.Email, req.Password, ct))
        {
            ThrowError("The supplied credentials are invalid!");
        }

        var jwtToken = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = "2D9CECE5-5E76-461D-98A3-2B1E9261B2ED";
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
                o.User.Roles.Add("Manager", "Auditor");
                o.User.Claims.Add(("UserName", req.Email));
                o.User["UserId"] = "001"; //indexer based claim setting
            });

        await SendAsync(new Response{ Token = jwtToken}, cancellation: ct);
    }
}

public record Request
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record Response
{
    public required string Token { get; init; }
}

public static class AuthService
{
    public static async Task<bool> CredentialsAreValid(string email, string password, CancellationToken ct)
    {
        return true;
    }
}
