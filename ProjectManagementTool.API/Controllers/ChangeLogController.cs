using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.Interfaces.Services;

[ApiController]
[Route("api/logs")]
public class ChangeLogController : ControllerBase
{
    private readonly IChangeLogService _logService;

    public ChangeLogController(IChangeLogService logService)
    {
        _logService = logService;
    }

    [HttpGet("project/{projectId}/by/{requesterId}")]
    public async Task<IActionResult> ProjectLogs(Guid projectId, Guid requesterId)
    {
        var logs = await _logService.GetProjectLogsAsync(projectId, requesterId);
        return Ok(logs);
    }

    [HttpGet("team/{teamId}/by/{requesterId}")]
    public async Task<IActionResult> TeamLogs(Guid teamId, Guid requesterId)
    {
        var logs = await _logService.GetTeamLogsAsync(teamId, requesterId);
        return Ok(logs);
    }

    [HttpGet("task/{taskId}/by/{requesterId}")]
    public async Task<IActionResult> TaskLogs(Guid taskId, Guid requesterId)
    {
        var logs = await _logService.GetTaskLogsAsync(taskId, requesterId);
        return Ok(logs);
    }
}