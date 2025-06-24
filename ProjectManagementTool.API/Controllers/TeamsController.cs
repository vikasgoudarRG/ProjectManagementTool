using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.DTOs.Team;

[ApiController]
[Route("api/teams")]
public class TeamController : ControllerBase
{
    // ======================= Fields ======================= //
    private readonly ITeamService _teamService;

    // ==================== Constructors ==================== //
    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    // ======================= Methods ====================== //
    // Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeamDTO createTeamDto)
    {
        Guid teamId = await _teamService.CreateTeamAsync(dto);
        return Ok(teamId);
    }

    [HttpPost("{teamId}/add-member")]
    public async Task<IActionResult> AddMember(Guid teamId, [FromBody] TeamMemberActionDTO dto)
    {
        dto.TeamId = teamId;
        await _teamService.AddMemberAsync(dto);
        return NoContent();
    }

    [HttpPost("{teamId}/assign-lead")]
    public async Task<IActionResult> AssignLead(Guid teamId, [FromBody] TeamMemberActionDTO dto)
    {
        dto.TeamId = teamId;
        await _teamService.AssignTeamLeadAsync(dto);
        return NoContent();
    }

    // Read
    [HttpGet("{teamId}")]
    public async Task<IActionResult> Get(Guid teamId, [FromQuery] Guid requesterId)
    {
        var team = await _teamService.GetByIdAsync(teamId, requesterId);
        return team is null ? NotFound() : Ok(team);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetAllByProject(Guid projectId, [FromQuery] Guid requesterId)
    {
        var teams = await _teamService.GetAllByProjectIdAsync(projectId, requesterId);
        return Ok(teams);
    }

    // Update
    

    // Delete
    [HttpDelete("{teamId}/remove-member/{userId}")]
    public async Task<IActionResult> RemoveMember(Guid teamId, Guid userId, [FromQuery] Guid requesterId)
    {
        var dto = new TeamMemberActionDTO { TeamId = teamId, UserId = userId, RequesterId = requesterId };
        await _teamService.RemoveMemberAsync(dto);
        return NoContent();
    }
    
    // Remove team lead
    [HttpDelete("{teamId}/remove-lead")]
    public async Task<IActionResult> RemoveLead(Guid teamId, [FromQuery] Guid requesterId)
    {
        var dto = new TeamMemberActionDTO { TeamId = teamId, RequesterId = requesterId };
        await _teamService.RemoveTeamLeadAsync(dto);
        return NoContent();
    }

    // Delete team
    [HttpDelete("{teamId}")]
    public async Task<IActionResult> Delete(Guid teamId, [FromQuery] Guid requesterId)
    {
        await _teamService.DeleteTeamAsync(teamId, requesterId);
        return NoContent();
    }
}