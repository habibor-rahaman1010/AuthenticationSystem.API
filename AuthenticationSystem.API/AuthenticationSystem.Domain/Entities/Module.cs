using AuthenticationSystem.Domain.Common;

namespace AuthenticationSystem.Domain.Entities;

public class Module : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
