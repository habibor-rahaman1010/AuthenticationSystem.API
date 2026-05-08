using AuthenticationSystem.Domain.Common;

namespace AuthenticationSystem.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid ModuleId { get; set; }
    public string? Description { get; set; }
}
