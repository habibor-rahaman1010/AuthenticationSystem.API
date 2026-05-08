using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.Permission;

public class CreatePermissionDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Guid ModuleId { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
