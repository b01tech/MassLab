using MassLib.Identity.Domain.Entities;
using MassLib.Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MassLib.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IIdentityDbContext _context;

    public UserRepository(IIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        // Assuming UserName is used as email for login
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName.Value == email, cancellationToken);
    }
    
    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName.Value == userName, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        // SaveChanges is handled by UnitOfWork in Handler
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        // SaveChanges is handled by UnitOfWork in Handler
        return Task.CompletedTask;
    }
}
