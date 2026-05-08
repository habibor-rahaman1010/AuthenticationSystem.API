using AuthenticationSystem.Application.DTOs.User;
using AuthenticationSystem.Application.Interfaces.Repositories;
using AuthenticationSystem.Application.Interfaces.Services;

namespace AuthenticationSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userRepository.GetUserRolesAsync(user.Id);
            result.Add(new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                Roles = roles
            });
        }

        return result;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null) return null;

        var roles = await _userRepository.GetUserRolesAsync(id);

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            Roles = roles
        };
    }

    public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null) return false;

        if (dto.Username is not null) user.Username = dto.Username;
        if (dto.Email is not null) user.Email = dto.Email;
        if (dto.IsActive.HasValue) user.IsActive = dto.IsActive.Value;

        user.UpdatedAt = DateTime.UtcNow;

        return await _userRepository.UpdateAsync(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}
