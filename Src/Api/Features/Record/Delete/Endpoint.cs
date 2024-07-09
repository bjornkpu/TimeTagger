using Api.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Record.Delete;

[AllowAnonymous]
[HttpDelete("/record/{id}")]
public class Endpoint : EndpointWithoutRequest
{
    public required DbCtx DbCtx { get; set; }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var recordId = Route<Guid>("id");
        var record = await DbCtx.Records.FirstOrDefaultAsync(e => e.Id == recordId,ct);

        if (record is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        DbCtx.Records.Remove(record);
        await DbCtx.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}
