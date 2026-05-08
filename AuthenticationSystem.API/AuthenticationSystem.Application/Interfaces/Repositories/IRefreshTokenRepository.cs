using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<Guid> AddAsync(UserToken token);
    Task<UserToken?> GetByTokenAsync(string refreshToken);
    Task<bool> RevokeAsync(Guid tokenId);
    Task<bool> RevokeAllByUserAsync(Guid userId);
    Task<IEnumerable<UserToken>> GetActiveTokensByUserAsync(Guid userId);
}
