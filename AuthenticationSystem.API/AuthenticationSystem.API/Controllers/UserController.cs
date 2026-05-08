using AuthenticationSystem.API.Authorization;
using AuthenticationSystem.Application.Constants;
using AuthenticationSystem.Application.DTOs.User;
using AuthenticationSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [RequirePermission(AppPermissions.Users.View)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission(AppPermissions.Users.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(AppPermissions.Users.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
    {
        var result = await _userService.UpdateUserAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(AppPermissions.Users.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        return result ? NoContent() : NotFound();
    }
}
