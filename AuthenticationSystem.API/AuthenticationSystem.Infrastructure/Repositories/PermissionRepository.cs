using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Domain.Entities;
using AuthenticationSystem.Infrastructure.Data;
using Dapper;

namespace AuthenticationSystem.Infrastructure.Repositories;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    protected override string TableName => "Permissions";
    protected override string PrimaryKeyColumn => "PermissionId";
    protected override string SelectSql =>
        "SELECT PermissionId AS Id, PermissionName AS Name, ModuleId, Description, CreatedAt FROM Permissions";

    public PermissionRepository(DapperContext context) : base(context) { }

    public override async Task<Guid> AddAsync(Permission entity)
    {
        const string sql = """
            INSERT INTO Permissions (PermissionId, PermissionName, ModuleId, Description, CreatedAt)
            VALUES (@Id, @Name, @ModuleId, @Description, @CreatedAt);
            """;

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, entity);
        return entity.Id;
    }

    public override async Task<bool> UpdateAsync(Permission entity)
    {
        const string sql = """
            UPDATE Permissions
            SET PermissionName = @Name,
                ModuleId       = @ModuleId,
                Description    = @Description
            WHERE PermissionId = @Id
            """;

        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, entity);
        return rows > 0;
    }

    public async Task<IEnumerable<Permission>> GetByModuleIdAsync(Guid moduleId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Permission>(
            $"{SelectSql} WHERE ModuleId = @ModuleId", new { ModuleId = moduleId });
    }

    public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
    {
        const string sql = """
            IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId)
                INSERT INTO RolePermissions (RolePermissionId, RoleId, PermissionId, AssignedAt)
                VALUES (@RolePermissionId, @RoleId, @PermissionId, @AssignedAt);
            """;

        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = roleId,
            PermissionId = permissionId,
            AssignedAt = DateTime.UtcNow
        });
        return rows > 0;
    }

    public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
    {
        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "DELETE FROM RolePermissions WHERE RoleId = @RoleId AND PermissionId = @PermissionId",
            new { RoleId = roleId, PermissionId = permissionId });
        return rows > 0;
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
    {
        const string sql = """
            SELECT p.PermissionId AS Id, p.PermissionName AS Name, p.ModuleId, p.Description, p.CreatedAt
            FROM Permissions p
            INNER JOIN RolePermissions rp ON rp.PermissionId = p.PermissionId
            WHERE rp.RoleId = @RoleId
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Permission>(sql, new { RoleId = roleId });
    }
}
