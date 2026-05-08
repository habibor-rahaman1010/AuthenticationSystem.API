using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Interfaces.Repositories;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
    Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId);
    Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);
}
