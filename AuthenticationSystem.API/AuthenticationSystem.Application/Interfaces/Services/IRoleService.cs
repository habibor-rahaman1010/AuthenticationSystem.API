using AuthenticationSystem.Application.DTOs.Role;

namespace AuthenticationSystem.Application.Interfaces.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(Guid id);
    Task<RoleDto> CreateRoleAsync(CreateRoleDto dto);
    Task<bool> UpdateRoleAsync(Guid id, UpdateRoleDto dto);
    Task<bool> DeleteRoleAsync(Guid id);
    Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId);
    Task<IEnumerable<RoleDto>> GetRolesByUserIdAsync(Guid userId);
}
