using AuthenticationSystem.Application.DTOs.Auth;
using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Application.Interfaces.Services;
using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("Email is already registered.");

        if (await _userRepository.GetByUsernameAsync(request.Username) is not null)
            throw new InvalidOperationException("Username is already taken.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var userId = await _userRepository.AddAsync(user);

        var userRole = await _roleRepository.GetByNameAsync("User");
        if (userRole is not null)
            await _roleRepository.AssignRoleToUserAsync(userId, userRole.Id);

        var roles = await _userRepository.GetUserRolesAsync(userId);
        var refreshToken = await CreateRefreshTokenAsync(userId);

        return new AuthResponseDto
        {
            AccessToken = _tokenService.GenerateAccessToken(user, roles),
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
            UserId = userId,
            Username = user.Username,
            Email = user.Email,
            Roles = roles
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is disabled.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        await _refreshTokenRepository.RevokeAllByUserAsync(user.Id);

        var roles = await _userRepository.GetUserRolesAsync(user.Id);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);

        return new AuthResponseDto
        {
            AccessToken = _tokenService.GenerateAccessToken(user, roles),
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Roles = roles
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (token.ExpiryDate <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token has expired.");

        var user = await _userRepository.GetByIdAsync(token.UserId)
            ?? throw new UnauthorizedAccessException("User not found.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is disabled.");

        await _refreshTokenRepository.RevokeAsync(token.Id);

        var roles = await _userRepository.GetUserRolesAsync(user.Id);
        var newRefreshToken = await CreateRefreshTokenAsync(user.Id);

        return new AuthResponseDto
        {
            AccessToken = _tokenService.GenerateAccessToken(user, roles),
            RefreshToken = newRefreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Roles = roles
        };
    }

    public async Task<bool> RevokeTokenAsync(Guid userId)
    {
        return await _refreshTokenRepository.RevokeAllByUserAsync(userId);
    }

    private async Task<string> CreateRefreshTokenAsync(Guid userId)
    {
        var token = new UserToken
        {
            UserId = userId,
            RefreshToken = _tokenService.GenerateRefreshToken(),
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(token);
        return token.RefreshToken;
    }
}
