using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Services;

namespace ProjectManagementTool.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    // ======================= Fields ======================= //
    private readonly IProjectService _projectService;

    // ==================== Constructors ==================== //
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // ======================= Methods ====================== //
    // Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDTO createProjectDto)
    {
        ProjectDTO projectDto = await _projectService.CreateProjectAsync(createProjectDto);
        return CreatedAtAction(
            nameof(GetById),
            new {projectId = projectDto.Id},
            projectDto
        );
    }

    // Update
    [HttpPost("add-developer")]
    public async Task<IActionResult> AddDeveloper([FromQuery] Guid requestorId, [FromBody] ProjectDeveloperDTO dto)
    {
        await _projectService.AddDeveloperAsync(requestorId, dto);
        return NoContent();
    }

    [HttpPost("remove-developer")]
    public async Task<IActionResult> RemoveDeveloper([FromQuery] Guid requestorId, [FromBody] ProjectDeveloperDTO dto)
    {
        await _projectService.RemoveDeveloperAsync(requestorId, dto);
        return NoContent();
    }

    [HttpPut("{projectId}")]
    public async Task<IActionResult> GetById([FromQuery] Guid requestorId, [FromRoute] Guid projectId, [FromBody] UpdateProjectDTO dto)
    {
        await _projectService.UpdateAsync(requestorId, projectId, dto);
        return NoContent();
    }

    // Read
    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetById(Guid projectId)
    {
        ProjectDTO project = await _projectService.GetByIdAsync(projectId);
        return Ok(project);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllForUser(Guid userId)
    {
        IEnumerable<ProjectDTO> projects = await _projectService.GetAllForUserAsync(userId);
        return Ok(projects);
    }

    [HttpGet("{projectId}/developers/{requesterId}")]
    public async Task<IActionResult> GetAllDevelopers(Guid projectId, Guid requesterId)
    {
        IEnumerable<UserDTO> developers = await _projectService.GetAllDevelopersAsync(projectId, requesterId);
        return Ok(developers);
    }

    // Delete
    [HttpDelete("{projectId}/by/{requesterId}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid requesterId)
    {
        await _projectService.DeleteProjectAsync(projectId, requesterId);
        return NoContent();
    }
}
