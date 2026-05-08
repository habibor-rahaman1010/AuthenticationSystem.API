using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Domain.Entities;
using AuthenticationSystem.Infrastructure.Data;
using Dapper;

namespace AuthenticationSystem.Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    protected override string TableName => "Roles";
    protected override string PrimaryKeyColumn => "RoleId";
    protected override string SelectSql =>
        "SELECT RoleId AS Id, RoleName AS Name, Description, CreatedAt FROM Roles";

    public RoleRepository(DapperContext context) : base(context) { }

    public override async Task<Guid> AddAsync(Role entity)
    {
        const string sql = """
            INSERT INTO Roles (RoleId, RoleName, Description, CreatedAt)
            VALUES (@Id, @Name, @Description, @CreatedAt);
            """;

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, entity);
        return entity.Id;
    }

    public override async Task<bool> UpdateAsync(Role entity)
    {
        const string sql = """
            UPDATE Roles
            SET RoleName    = @Name,
                Description = @Description
            WHERE RoleId = @Id
            """;

        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, entity);
        return rows > 0;
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Role>(
            $"{SelectSql} WHERE RoleName = @Name", new { Name = name });
    }

    public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
    {
        const string sql = """
            IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
                INSERT INTO UserRoles (UserRoleId, UserId, RoleId, AssignedAt)
                VALUES (@UserRoleId, @UserId, @RoleId, @AssignedAt);
            """;

        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new
        {
            UserRoleId = Guid.NewGuid(),
            UserId = userId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow
        });
        return rows > 0;
    }

    public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
    {
        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId",
            new { UserId = userId, RoleId = roleId });
        return rows > 0;
    }

    public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId)
    {
        const string sql = """
            SELECT r.RoleId AS Id, r.RoleName AS Name, r.Description, r.CreatedAt
            FROM Roles r
            INNER JOIN UserRoles ur ON ur.RoleId = r.RoleId
            WHERE ur.UserId = @UserId
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Role>(sql, new { UserId = userId });
    }
}
