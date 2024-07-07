using Microsoft.EntityFrameworkCore;

namespace TimeTagger.Api;

public class DbCtx(DbContextOptions<DbCtx> options) : DbContext(options);
