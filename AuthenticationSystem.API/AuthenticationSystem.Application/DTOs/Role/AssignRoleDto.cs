using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.Role;

public class AssignRoleDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid RoleId { get; set; }
}
