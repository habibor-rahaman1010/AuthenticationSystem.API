using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Domain.Entities;
using AuthenticationSystem.Infrastructure.Data;
using Dapper;

namespace AuthenticationSystem.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly DapperContext _context;

    private const string SelectSql =
        "SELECT LogId AS Id, UserId, Action, TableName, RecordId, OldValues, NewValues, IPAddress, CreatedAt FROM AuditLogs";

    public AuditLogRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog log)
    {
        const string sql = """
            INSERT INTO AuditLogs (LogId, UserId, Action, TableName, RecordId, OldValues, NewValues, IPAddress, CreatedAt)
            VALUES (@Id, @UserId, @Action, @TableName, @RecordId, @OldValues, @NewValues, @IPAddress, @CreatedAt);
            """;

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, log);
    }

    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<AuditLog>(
            $"{SelectSql} WHERE UserId = @UserId ORDER BY CreatedAt DESC",
            new { UserId = userId });
    }
}
