using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Interfaces.Repositories;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<IEnumerable<Permission>> GetByModuleIdAsync(Guid moduleId);
    Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
    Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
    Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
}
