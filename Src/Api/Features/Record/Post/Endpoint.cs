using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Api.Features.Record.Post;

[AllowAnonymous]
[HttpPost("/api/user/create")]
public class Endpoint : Endpoint<Request, Response>
{
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await SendAsync(new()
        {
            FullName = req.FirstName + " " + req.LastName,
            IsOver18 = req.Age > 18
        });
    }

}
public class Request
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

public class Response
{
    public string FullName { get; set; }
    public bool IsOver18 { get; set; }
}
