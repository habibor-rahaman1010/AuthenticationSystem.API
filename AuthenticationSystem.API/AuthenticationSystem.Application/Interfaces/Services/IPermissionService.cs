using AuthenticationSystem.Application.DTOs.Permission;

namespace AuthenticationSystem.Application.Interfaces.Services;

public interface IPermissionService
{
    Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
    Task<PermissionDto?> GetPermissionByIdAsync(Guid id);
    Task<IEnumerable<PermissionDto>> GetPermissionsByModuleAsync(Guid moduleId);
    Task<IEnumerable<PermissionDto>> GetPermissionsByRoleAsync(Guid roleId);
    Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto dto);
    Task<bool> UpdatePermissionAsync(Guid id, UpdatePermissionDto dto);
    Task<bool> DeletePermissionAsync(Guid id);
    Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
    Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
}
