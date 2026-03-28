using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

/// <summary>
/// Authentication service for registration and login.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email, ct);
        if (existing != null)
            return null;

        var user = new User
        {
            Email = dto.Email.ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Address = dto.Address,
            Role = "Customer"
        };

        var created = await _userRepository.AddAsync(user, ct);
        var token = _tokenService.GenerateToken(created);

        return new AuthResponseDto
        {
            Token = token,
            Email = created.Email,
            FirstName = created.FirstName,
            LastName = created.LastName,
            Role = created.Role,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email.ToLowerInvariant(), ct);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }
}
