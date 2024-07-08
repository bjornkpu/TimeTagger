using Microsoft.EntityFrameworkCore;

namespace Api;

public class DbCtx(DbContextOptions<DbCtx> options) : DbContext(options);
