using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.Role;

public class CreateRoleDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}
