using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

/// <summary>
/// Service interface for authentication operations.
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto dto, CancellationToken ct = default);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto, CancellationToken ct = default);
}
