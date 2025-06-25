using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.DTOs.Team;

namespace ProjectManagementTool.API.Controllers
{
    [ApiController]
    [Route("api/teams")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDTO createTeamDto)
        {
            TeamDTO teamDto = await _teamService.CreateTeamAsync(createTeamDto);
            return CreatedAtAction(nameof(Get), new { teamId = teamDto.Id, requesterId = createTeamDto.RequesterId }, teamDto);
        }

        [HttpPost("{teamId:guid}/members")]
        public async Task<IActionResult> AddMember(Guid teamId, [FromBody] AddMemberDTO dto)
        {
            await _teamService.AddMemberAsync(teamId, dto);
            return NoContent();
        }

        [HttpGet("{teamId:guid}")]
        public async Task<IActionResult> Get(Guid teamId, [FromQuery] Guid requesterId)
        {
            TeamDTO team = await _teamService.GetByIdAsync(teamId, requesterId);
            return Ok(team);
        }

        [HttpGet("/api/projects/{projectId:guid}/teams")]
        public async Task<IActionResult> GetAllByProject(Guid projectId, [FromQuery] Guid requesterId)
        {
            IEnumerable<TeamDTO> teams = await _teamService.GetAllByProjectIdAsync(projectId, requesterId);
            return Ok(teams);
        }

        [HttpPut("{teamId:guid}/lead")]
        public async Task<IActionResult> AssignLead(Guid teamId, [FromBody] AssignLeadDTO dto)
        {
            await _teamService.AssignTeamLeadAsync(teamId, dto);
            return NoContent();
        }

        [HttpDelete("{teamId:guid}/members/{userId:guid}")]
        public async Task<IActionResult> RemoveMember(Guid teamId, Guid userId, [FromQuery] Guid requesterId)
        {
            await _teamService.RemoveMemberAsync(teamId, userId, requesterId);
            return NoContent();
        }

        [HttpDelete("{teamId:guid}/lead")]
        public async Task<IActionResult> RemoveLead(Guid teamId, [FromQuery] Guid requesterId, [FromQuery] Guid userId)
        {
            AssignLeadDTO dto = new AssignLeadDTO { RequesterId = requesterId, UserId = userId };
            await _teamService.RemoveTeamLeadAsync(teamId, dto);
            return NoContent();
        }

        [HttpDelete("{teamId:guid}")]
        public async Task<IActionResult> Delete(Guid teamId, [FromQuery] Guid requesterId)
        {
            await _teamService.DeleteTeamAsync(teamId, requesterId);
            return NoContent();
        }
    }
}
