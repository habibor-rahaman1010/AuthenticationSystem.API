namespace AuthenticationSystem.Application.DTOs.Permission;

public class PermissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ModuleId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
