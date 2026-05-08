using AuthenticationSystem.Application.DTOs.Module;

namespace AuthenticationSystem.Application.Interfaces.Services;

public interface IModuleService
{
    Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
    Task<ModuleDto?> GetModuleByIdAsync(Guid id);
    Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto);
    Task<bool> UpdateModuleAsync(Guid id, UpdateModuleDto dto);
    Task<bool> DeleteModuleAsync(Guid id);
}
