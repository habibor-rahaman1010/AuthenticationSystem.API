using AuthenticationSystem.Application.DTOs.Permission;
using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Application.Interfaces.Services;
using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
    {
        var permissions = await _permissionRepository.GetAllAsync();
        return permissions.Select(MapToDto);
    }

    public async Task<PermissionDto?> GetPermissionByIdAsync(Guid id)
    {
        var permission = await _permissionRepository.GetByIdAsync(id);
        return permission is null ? null : MapToDto(permission);
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsByModuleAsync(Guid moduleId)
    {
        var permissions = await _permissionRepository.GetByModuleIdAsync(moduleId);
        return permissions.Select(MapToDto);
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsByRoleAsync(Guid roleId)
    {
        var permissions = await _permissionRepository.GetPermissionsByRoleIdAsync(roleId);
        return permissions.Select(MapToDto);
    }

    public async Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto dto)
    {
        var permission = new Permission
        {
            Name = dto.Name,
            ModuleId = dto.ModuleId,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        var id = await _permissionRepository.AddAsync(permission);
        permission.Id = id;

        return MapToDto(permission);
    }

    public async Task<bool> UpdatePermissionAsync(Guid id, UpdatePermissionDto dto)
    {
        var permission = await _permissionRepository.GetByIdAsync(id);
        if (permission is null) return false;

        if (dto.Name is not null) permission.Name = dto.Name;
        if (dto.ModuleId.HasValue) permission.ModuleId = dto.ModuleId.Value;
        if (dto.Description is not null) permission.Description = dto.Description;

        return await _permissionRepository.UpdateAsync(permission);
    }

    public async Task<bool> DeletePermissionAsync(Guid id)
    {
        return await _permissionRepository.DeleteAsync(id);
    }

    public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
    {
        return await _permissionRepository.AssignPermissionToRoleAsync(roleId, permissionId);
    }

    public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
    {
        return await _permissionRepository.RemovePermissionFromRoleAsync(roleId, permissionId);
    }

    private static PermissionDto MapToDto(Permission permission) => new()
    {
        Id = permission.Id,
        Name = permission.Name,
        ModuleId = permission.ModuleId,
        Description = permission.Description,
        CreatedAt = permission.CreatedAt
    };
}
