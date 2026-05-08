using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.Permission;

public class UpdatePermissionDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    public Guid? ModuleId { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
