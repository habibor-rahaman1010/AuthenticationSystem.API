using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Infrastructure.Data;
using Dapper;

namespace AuthenticationSystem.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DapperContext _context;
    protected abstract string TableName { get; }
    protected abstract string PrimaryKeyColumn { get; }
    protected abstract string SelectSql { get; }

    protected GenericRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<T>(SelectSql);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(
            $"{SelectSql} WHERE {PrimaryKeyColumn} = @Id", new { Id = id });
    }

    public abstract Task<Guid> AddAsync(T entity);

    public abstract Task<bool> UpdateAsync(T entity);

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = _context.CreateConnection();
        var rows = await connection.ExecuteAsync(
            $"DELETE FROM {TableName} WHERE {PrimaryKeyColumn} = @Id", new { Id = id });
        return rows > 0;
    }
}
