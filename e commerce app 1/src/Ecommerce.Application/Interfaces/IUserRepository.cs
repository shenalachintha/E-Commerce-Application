using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Repository interface for User data access.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User> AddAsync(User user, CancellationToken ct = default);
}
