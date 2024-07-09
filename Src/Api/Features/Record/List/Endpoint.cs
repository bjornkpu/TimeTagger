using Api.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Record.List;

[AllowAnonymous]
[HttpGet("/record")]
public class Endpoint : EndpointWithoutRequest<List<Database.Record>>
{
    public required DbCtx DbCtx { get; set; }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var records = await DbCtx.Records.ToListAsync(cancellationToken: ct);
        await SendAsync(records, cancellation: ct);
    }

}
