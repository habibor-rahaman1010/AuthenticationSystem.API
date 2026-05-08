using Microsoft.AspNetCore.Authorization;

namespace AuthenticationSystem.API.Authorization;

public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission) : base(permission) { }
}
