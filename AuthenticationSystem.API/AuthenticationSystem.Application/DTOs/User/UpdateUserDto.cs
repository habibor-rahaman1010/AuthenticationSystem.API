using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.User;

public class UpdateUserDto
{
    [MaxLength(100)]
    public string? Username { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    public bool? IsActive { get; set; }
}
