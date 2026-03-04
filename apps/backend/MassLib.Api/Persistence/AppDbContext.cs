using Microsoft.EntityFrameworkCore;

namespace MassLib.Api.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public Task CommitAsync() => SaveChangesAsync();
}
