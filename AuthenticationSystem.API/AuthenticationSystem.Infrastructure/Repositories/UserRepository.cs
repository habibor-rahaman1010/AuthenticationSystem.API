using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Domain.Entities;
using AuthenticationSystem.Infrastructure.Data;
using Dapper;

namespace AuthenticationSystem.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    protected override string TableName => "Users";
    protected override string PrimaryKeyColumn => "UserId";
    protected override string SelectSql =>
        "SELECT UserId AS Id, UserName AS Username, Email, PasswordHash, IsActive, CreatedAt, UpdatedAt FROM Users";

    public UserRepository(DapperContext context) : base(context) { }

    public override async Task<Guid> AddAsync(User entity)
    {
        const string sql = """
            INSERT INTO Users (UserId, UserName, Email, PasswordHash, IsActive, CreatedAt)
            VALUES (@Id, @Username, @Email, @PasswordHash, @IsActive, @CreatedAt);
            """;

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, entity);
        return entity.Id;
    }

    public override async Task<bool> UpdateAsync(User entity)
    {
        const string sql = """
            UPDATE Users
            SET UserName    = @Username,
                Email       = @Email,
                IsActive    = @IsActive,
                UpdatedAt   = @UpdatedAt
            WHERE UserId = @Id
            """;

        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, entity);
        return rows > 0;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            $"{SelectSql} WHERE Email = @Email", new { Email = email });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            $"{SelectSql} WHERE UserName = @Username", new { Username = username });
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
    {
        const string sql = """
            SELECT r.RoleName
            FROM Roles r
            INNER JOIN UserRoles ur ON ur.RoleId = r.RoleId
            WHERE ur.UserId = @UserId
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<string>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId)
    {
        const string sql = """
            SELECT DISTINCT p.PermissionName
            FROM Permissions p
            INNER JOIN RolePermissions rp ON rp.PermissionId = p.PermissionId
            INNER JOIN UserRoles ur ON ur.RoleId = rp.RoleId
            WHERE ur.UserId = @UserId
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<string>(sql, new { UserId = userId });
    }
}
