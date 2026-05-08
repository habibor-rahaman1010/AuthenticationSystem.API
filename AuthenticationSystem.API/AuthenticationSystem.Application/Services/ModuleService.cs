using AuthenticationSystem.Application.DTOs.Module;
using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Application.Interfaces.Services;
using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Services;

public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepository;

    public ModuleService(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
    {
        var modules = await _moduleRepository.GetAllAsync();
        return modules.Select(MapToDto);
    }

    public async Task<ModuleDto?> GetModuleByIdAsync(Guid id)
    {
        var module = await _moduleRepository.GetByIdAsync(id);
        return module is null ? null : MapToDto(module);
    }

    public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto)
    {
        var existing = await _moduleRepository.GetByNameAsync(dto.Name);
        if (existing is not null)
            throw new InvalidOperationException($"Module '{dto.Name}' already exists.");

        var module = new Module
        {
            Name = dto.Name,
            Description = dto.Description
        };

        var id = await _moduleRepository.AddAsync(module);
        module.Id = id;

        return MapToDto(module);
    }

    public async Task<bool> UpdateModuleAsync(Guid id, UpdateModuleDto dto)
    {
        var module = await _moduleRepository.GetByIdAsync(id);
        if (module is null) return false;

        if (dto.Name is not null) module.Name = dto.Name;
        if (dto.Description is not null) module.Description = dto.Description;

        return await _moduleRepository.UpdateAsync(module);
    }

    public async Task<bool> DeleteModuleAsync(Guid id)
    {
        return await _moduleRepository.DeleteAsync(id);
    }

    private static ModuleDto MapToDto(Module module) => new()
    {
        Id = module.Id,
        Name = module.Name,
        Description = module.Description
    };
}
