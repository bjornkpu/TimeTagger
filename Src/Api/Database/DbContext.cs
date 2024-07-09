using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class DbCtx(DbContextOptions<DbCtx> options) : DbContext(options)
{
    public DbSet<Record> Records { get; set; }
}

public class Record
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Tag { get; set; }
}
