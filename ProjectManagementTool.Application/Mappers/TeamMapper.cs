using ProjectManagementTool.Application.DTOs.Team;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public class TeamMapper : ITeamMapper
    {
        public TeamDTO ToDTO(Team team)
        {
            return new TeamDTO
            {
                Id = team.Id,
                Name = team.Name,
                ProjectId = team.ProjectId,
                TeamMember = team.TeamMembers.Select(tm => new TeamMemberDTO { userId = tm.UserId, Role = tm.Role }),
                CreatedOn = team.CreatedOn  
            };
        }
    }
}