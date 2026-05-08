using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Domain.Entities;
using AuthenticationSystem.Infrastructure.Data;
using Dapper;

namespace AuthenticationSystem.Infrastructure.Repositories;

public class ModuleRepository : GenericRepository<Module>, IModuleRepository
{
    protected override string TableName => "Modules";
    protected override string PrimaryKeyColumn => "ModuleId";
    protected override string SelectSql =>
        "SELECT ModuleId AS Id, ModuleName AS Name, Description FROM Modules";

    public ModuleRepository(DapperContext context) : base(context) { }

    public override async Task<Guid> AddAsync(Module entity)
    {
        const string sql = """
            INSERT INTO Modules (ModuleId, ModuleName, Description)
            VALUES (@Id, @Name, @Description);
            """;

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, entity);
        return entity.Id;
    }

    public override async Task<bool> UpdateAsync(Module entity)
    {
        const string sql = """
            UPDATE Modules
            SET ModuleName  = @Name,
                Description = @Description
            WHERE ModuleId = @Id
            """;

        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, entity);
        return rows > 0;
    }

    public async Task<Module?> GetByNameAsync(string moduleName)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Module>(
            $"{SelectSql} WHERE ModuleName = @Name", new { Name = moduleName });
    }
}
