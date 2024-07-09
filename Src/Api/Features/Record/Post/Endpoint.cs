using Api.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Api.Features.Record.Post;

[AllowAnonymous]
[HttpPost("/record")]
public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public required DbCtx DbCtx { get; set; }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var res = await DbCtx.Records.AddAsync(new Database.Record()
        {
            Id = new Guid(),
            Timestamp = DateTime.UtcNow,
            Tag = req.Tag
        }, ct);
        await DbCtx.SaveChangesAsync(ct);

        await SendAsync(Map.FromEntity(res.Entity), cancellation: ct);
    }

}
public class Request
{
    public string? Tag { get; set; }
}

public class Response : Database.Record;


public class Mapper : Mapper<Request, Response, Database.Record>
{
    public override Response FromEntity(Database.Record e) => new()
    {
        Id = e.Id,
        Timestamp = e.Timestamp.ToLocalTime(),
        Tag = e.Tag
    };
}
