using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
    Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId);
}
