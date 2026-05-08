using AuthenticationSystem.API.Authorization;
using AuthenticationSystem.Application.Constants;
using AuthenticationSystem.Application.DTOs.Module;
using AuthenticationSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ModuleController : ControllerBase
{
    private readonly IModuleService _moduleService;

    public ModuleController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    [HttpGet]
    [RequirePermission(AppPermissions.Modules.View)]
    public async Task<IActionResult> GetAll()
    {
        var modules = await _moduleService.GetAllModulesAsync();
        return Ok(modules);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission(AppPermissions.Modules.View)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var module = await _moduleService.GetModuleByIdAsync(id);
        return module is null ? NotFound() : Ok(module);
    }

    [HttpPost]
    [RequirePermission(AppPermissions.Modules.Create)]
    public async Task<IActionResult> Create([FromBody] CreateModuleDto dto)
    {
        var module = await _moduleService.CreateModuleAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = module.Id }, module);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission(AppPermissions.Modules.Edit)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateModuleDto dto)
    {
        var result = await _moduleService.UpdateModuleAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission(AppPermissions.Modules.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _moduleService.DeleteModuleAsync(id);
        return result ? NoContent() : NotFound();
    }
}
