using System.Security.Claims;
using AuthenticationSystem.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationSystem.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserRepository _userRepository;

    public PermissionAuthorizationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue("sub");

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            context.Fail();
            return;
        }

        var permissions = await _userRepository.GetUserPermissionsAsync(userId);

        if (permissions.Contains(requirement.Permission))
            context.Succeed(requirement);
        else
            context.Fail();
    }
}
