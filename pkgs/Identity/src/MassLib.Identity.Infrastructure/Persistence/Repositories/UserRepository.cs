using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MassLib.Identity.Infrastructure.Persistence.Repositories;

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

    public async Task<IEnumerable<User>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users.CountAsync(cancellationToken);
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
