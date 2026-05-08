using AuthenticationSystem.API.Authorization;
using AuthenticationSystem.Application.Constants;
using AuthenticationSystem.Application.DTOs.Role;
using AuthenticationSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [RequirePermission(AppPermissions.Roles.View)]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission(AppPermissions.Roles.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpPost]
    [RequirePermission(AppPermissions.Roles.Create)]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
    {
        var role = await _roleService.CreateRoleAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(AppPermissions.Roles.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto)
    {
        var result = await _roleService.UpdateRoleAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(AppPermissions.Roles.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _roleService.DeleteRoleAsync(id);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("user/{userId:guid}")]
    [RequirePermission(AppPermissions.Roles.View)]
    public async Task<IActionResult> GetRolesByUser(Guid userId)
    {
        var roles = await _roleService.GetRolesByUserIdAsync(userId);
        return Ok(roles);
    }

    [HttpPost("assign")]
    [RequirePermission(AppPermissions.Roles.Edit)]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto dto)
    {
        var result = await _roleService.AssignRoleToUserAsync(dto.UserId, dto.RoleId);
        return result ? NoContent() : BadRequest(new { message = "Failed to assign role." });
    }

    [HttpDelete("assign")]
    [RequirePermission(AppPermissions.Roles.Edit)]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] AssignRoleDto dto)
    {
        var result = await _roleService.RemoveRoleFromUserAsync(dto.UserId, dto.RoleId);
        return result ? NoContent() : BadRequest(new { message = "Failed to remove role." });
    }
}
