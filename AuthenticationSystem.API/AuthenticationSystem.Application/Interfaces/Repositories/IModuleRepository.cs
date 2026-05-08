using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Interfaces.Repositories;

public interface IModuleRepository : IGenericRepository<Module>
{
    Task<Module?> GetByNameAsync(string moduleName);
}
