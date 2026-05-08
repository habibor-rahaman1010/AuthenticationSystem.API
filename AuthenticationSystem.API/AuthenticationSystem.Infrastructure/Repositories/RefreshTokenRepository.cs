using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Domain.Entities;
using AuthenticationSystem.Infrastructure.Data;
using Dapper;

namespace AuthenticationSystem.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DapperContext _context;

    private const string SelectSql =
        "SELECT TokenId AS Id, UserId, RefreshToken, ExpiryDate, IsRevoked, CreatedAt FROM RefreshTokens";

    public RefreshTokenRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(UserToken token)
    {
        const string sql = """
            INSERT INTO RefreshTokens (TokenId, UserId, RefreshToken, ExpiryDate, IsRevoked, CreatedAt)
            VALUES (@Id, @UserId, @RefreshToken, @ExpiryDate, @IsRevoked, @CreatedAt);
            """;

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, token);
        return token.Id;
    }

    public async Task<UserToken?> GetByTokenAsync(string refreshToken)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<UserToken>(
            $"{SelectSql} WHERE RefreshToken = @RefreshToken AND IsRevoked = 0",
            new { RefreshToken = refreshToken });
    }

    public async Task<bool> RevokeAsync(Guid tokenId)
    {
        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "UPDATE RefreshTokens SET IsRevoked = 1 WHERE TokenId = @Id",
            new { Id = tokenId });
        return rows > 0;
    }

    public async Task<bool> RevokeAllByUserAsync(Guid userId)
    {
        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "UPDATE RefreshTokens SET IsRevoked = 1 WHERE UserId = @UserId",
            new { UserId = userId });
        return rows > 0;
    }

    public async Task<IEnumerable<UserToken>> GetActiveTokensByUserAsync(Guid userId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<UserToken>(
            $"{SelectSql} WHERE UserId = @UserId AND IsRevoked = 0 AND ExpiryDate > @Now",
            new { UserId = userId, Now = DateTime.UtcNow });
    }
}
