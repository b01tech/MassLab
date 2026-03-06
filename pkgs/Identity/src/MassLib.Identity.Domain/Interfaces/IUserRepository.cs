using MassLib.Identity.Domain.Entities;

namespace MassLib.Identity.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(int page, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}
