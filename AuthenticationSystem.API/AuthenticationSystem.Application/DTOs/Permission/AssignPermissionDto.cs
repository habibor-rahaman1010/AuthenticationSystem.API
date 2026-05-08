using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.Permission;

public class AssignPermissionDto
{
    [Required]
    public Guid RoleId { get; set; }

    [Required]
    public Guid PermissionId { get; set; }
}
