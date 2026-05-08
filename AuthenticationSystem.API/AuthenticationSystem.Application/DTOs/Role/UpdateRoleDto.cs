using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.Role;

public class UpdateRoleDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
