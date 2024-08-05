using Api.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Record.List;

[AllowAnonymous]
[HttpGet("/record")]
public class Endpoint : EndpointWithoutRequest<List<Response>,Mapper>
{
    public required DbCtx DbCtx { get; set; }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var records = await DbCtx.Records.ToListAsync(cancellationToken: ct);
        await SendAsync(records.Select(r => Map.FromEntity(r)).ToList(), cancellation: ct);
    }

}

public class Response : Database.Record;

public class Mapper : ResponseMapper<Response, Database.Record>
{
    public override Response FromEntity(Database.Record e) => new()
    {
        Id = e.Id,
        Timestamp = e.Timestamp.ToLocalTime(),
        Tag = e.Tag
    };
}
