using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.DTOs.Team;
using ProjectManagementTool.Application.DTOs.TaskItem;

[ApiController]
[Route("api/tasks")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskService;

    public TaskItemController(ITaskItemService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost("{creatorId}")]
    public async Task<IActionResult> Create([FromBody] TaskItemCreateDTO dto, Guid creatorId)
    {
        var id = await _taskService.CreateTaskAsync(dto, creatorId);
        return Ok(id);
    }

    [HttpGet("{id}/user/{requesterId}")]
    public async Task<IActionResult> Get(Guid id, Guid requesterId)
    {
        var task = await _taskService.GetByIdAsync(id, requesterId);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpGet("project/{projectId}/user/{requesterId}")]
    public async Task<IActionResult> ByProject(Guid projectId, Guid requesterId)
    {
        var tasks = await _taskService.GetByProjectAsync(projectId, requesterId);
        return Ok(tasks);
    }

    [HttpGet("team/{teamId}/user/{requesterId}")]
    public async Task<IActionResult> ByTeam(Guid teamId, Guid requesterId)
    {
        var tasks = await _taskService.GetByTeamAsync(teamId, requesterId);
        return Ok(tasks);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> ByUser(Guid userId)
    {
        var tasks = await _taskService.GetByUserAsync(userId);
        return Ok(tasks);
    }

    [HttpPut("{updaterId}")]
    public async Task<IActionResult> Update([FromBody] UpdateTaskItemDTO dto, Guid updaterId)
    {
        await _taskService.UpdateAsync(dto, updaterId);
        return NoContent();
    }

    [HttpDelete("{taskId}/delete-by/{requesterId}")]
    public async Task<IActionResult> Delete(Guid taskId, Guid requesterId)
    {
        await _taskService.DeleteAsync(taskId, requesterId);
        return NoContent();
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignTaskItemDTO dto)
    {
        await _taskService.AssignAsync(dto);
        return NoContent();
    }

    [HttpPost("status")]
    public async Task<IActionResult> ChangeStatus([FromBody] ChangeTaskItemStatusDTO dto)
    {
        await _taskService.ChangeStatusAsync(dto);
        return NoContent();
    }
}