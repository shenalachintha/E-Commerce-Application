using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Service interface for JWT token generation.
/// </summary>
public interface ITokenService
{
    string GenerateToken(User user);
}
