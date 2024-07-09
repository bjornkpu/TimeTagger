using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Api.Features.Record.PostRecord;

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
