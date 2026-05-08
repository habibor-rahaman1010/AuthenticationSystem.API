using AuthenticationSystem.API.Authorization;
using AuthenticationSystem.Application.Constants;
using AuthenticationSystem.Application.DTOs.Permission;
using AuthenticationSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    [RequirePermission(AppPermissions.Perms.View)]
    public async Task<IActionResult> GetAll()
    {
        var permissions = await _permissionService.GetAllPermissionsAsync();
        return Ok(permissions);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission(AppPermissions.Perms.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var permission = await _permissionService.GetPermissionByIdAsync(id);
        return permission is null ? NotFound() : Ok(permission);
    }

    [HttpGet("module/{moduleId:guid}")]
    [RequirePermission(AppPermissions.Perms.View)]
    public async Task<IActionResult> GetByModule(Guid moduleId)
    {
        var permissions = await _permissionService.GetPermissionsByModuleAsync(moduleId);
        return Ok(permissions);
    }

    [HttpGet("role/{roleId:guid}")]
    [RequirePermission(AppPermissions.Perms.View)]
    public async Task<IActionResult> GetByRole(Guid roleId)
    {
        var permissions = await _permissionService.GetPermissionsByRoleAsync(roleId);
        return Ok(permissions);
    }

    [HttpPost]
    [RequirePermission(AppPermissions.Perms.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto)
    {
        var permission = await _permissionService.CreatePermissionAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = permission.Id }, permission);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(AppPermissions.Perms.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionDto dto)
    {
        var result = await _permissionService.UpdatePermissionAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(AppPermissions.Perms.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _permissionService.DeletePermissionAsync(id);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("assign")]
    [RequirePermission(AppPermissions.Perms.Edit)]
    public async Task<IActionResult> AssignToRole([FromBody] AssignPermissionDto dto)
    {
        var result = await _permissionService.AssignPermissionToRoleAsync(dto.RoleId, dto.PermissionId);
        return result ? NoContent() : BadRequest(new { message = "Failed to assign permission." });
    }

    [HttpDelete("assign")]
    [RequirePermission(AppPermissions.Perms.Edit)]
    public async Task<IActionResult> RemoveFromRole([FromBody] AssignPermissionDto dto)
    {
        var result = await _permissionService.RemovePermissionFromRoleAsync(dto.RoleId, dto.PermissionId);
        return result ? NoContent() : BadRequest(new { message = "Failed to remove permission." });
    }
}
