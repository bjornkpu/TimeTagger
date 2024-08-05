using Api.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Record.Update;

[AllowAnonymous]
[HttpPatch("/record/{id}")]
public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public required DbCtx DbCtx { get; set; }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var recordId = Route<Guid>("id");
        var record = await DbCtx.Records.FirstOrDefaultAsync(e => e.Id == recordId,ct);

        if (record is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (req.Tag is not null)
        {
            record.Tag = req.Tag;
        }
        
        await DbCtx.SaveChangesAsync(ct);

        await SendAsync(Map.FromEntity(record), cancellation: ct);
    }

}
public class Request
{
    public string? Tag { get; set; }
    public DateTime? Timestamp { get; set; }
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
