using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Api.Features.HelloWorld;

[AllowAnonymous]
[HttpGet("/")]
public class Endpoint : EndpointWithoutRequest<Response>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new Response
        {
            Message = "Hello World! [TimeTagger]",
        }, cancellation: ct);
    }
}

public class Response
{
    public string Message { get; set; }
}
