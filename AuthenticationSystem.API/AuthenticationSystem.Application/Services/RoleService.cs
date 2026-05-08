using AuthenticationSystem.Application.DTOs.Role;
using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Application.Interfaces.Services;
using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(MapToDto);
    }

    public async Task<RoleDto?> GetRoleByIdAsync(Guid id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        return role is null ? null : MapToDto(role);
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto)
    {
        var existing = await _roleRepository.GetByNameAsync(dto.Name);
        if (existing is not null)
            throw new InvalidOperationException($"Role '{dto.Name}' already exists.");

        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        var id = await _roleRepository.AddAsync(role);
        role.Id = id;

        return MapToDto(role);
    }

    public async Task<bool> UpdateRoleAsync(Guid id, UpdateRoleDto dto)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role is null) return false;

        if (dto.Name is not null) role.Name = dto.Name;
        if (dto.Description is not null) role.Description = dto.Description;

        return await _roleRepository.UpdateAsync(role);
    }

    public async Task<bool> DeleteRoleAsync(Guid id)
    {
        return await _roleRepository.DeleteAsync(id);
    }

    public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
    {
        return await _roleRepository.AssignRoleToUserAsync(userId, roleId);
    }

    public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
    {
        return await _roleRepository.RemoveRoleFromUserAsync(userId, roleId);
    }

    public async Task<IEnumerable<RoleDto>> GetRolesByUserIdAsync(Guid userId)
    {
        var roles = await _roleRepository.GetRolesByUserIdAsync(userId);
        return roles.Select(MapToDto);
    }

    private static RoleDto MapToDto(Role role) => new()
    {
        Id = role.Id,
        Name = role.Name,
        Description = role.Description,
        CreatedAt = role.CreatedAt
    };
}
