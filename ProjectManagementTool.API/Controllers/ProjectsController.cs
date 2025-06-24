using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Services;

namespace ProjectManagementTool.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDTO dto)
    {
        var id = await _projectService.CreateProjectAsync(dto);
        return Ok(id);
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetById(Guid projectId)
    {
        var project = await _projectService.GetByIdAsync(projectId);
        return project == null ? NotFound() : Ok(project);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllForUser(Guid userId)
    {
        var projects = await _projectService.GetAllForUserAsync(userId);
        return Ok(projects);
    }

    [HttpDelete("{projectId}/by/{requesterId}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid requesterId)
    {
        await _projectService.DeleteProjectAsync(projectId, requesterId);
        return NoContent();
    }

    
    [HttpPost("add-developer")]
    public async Task<IActionResult> AddDeveloper([FromBody] ProjectUserActionDTO dto)
    {
        await _projectService.AddDeveloperAsync(dto);
        return NoContent();
    }

    [HttpPost("remove-developer")]
    public async Task<IActionResult> RemoveDeveloper([FromBody] ProjectUserActionDTO dto)
    {
        await _projectService.RemoveDeveloperAsync(dto);
        return NoContent();
    }

    [HttpGet("{projectId}/developers/{requesterId}")]
    public async Task<IActionResult> GetAllDevelopers(Guid projectId, Guid requesterId)
    {
        var developers = await _projectService.GetAllDevelopersAsync(projectId, requesterId);
        return Ok(developers);
    }
}
