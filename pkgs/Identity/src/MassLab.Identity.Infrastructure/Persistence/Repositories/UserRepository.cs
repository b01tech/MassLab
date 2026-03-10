using MassLab.Identity.Domain.Entities;
using MassLab.Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MassLab.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository(IIdentityDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.UserName.Value == userName, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u => u.UserName.Value.Contains(searchTerm));
        }

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var query = context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u => u.UserName.Value.Contains(searchTerm));
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }
}
