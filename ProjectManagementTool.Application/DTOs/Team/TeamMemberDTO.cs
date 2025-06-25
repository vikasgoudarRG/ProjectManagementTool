using ProjectManagementTool.Domain.Enums.Team;

namespace ProjectManagementTool.Application.DTOs.Team
{
    public class TeamMemberDTO
    {
        public Guid userId { get; set; }
        public TeamMemberRole Role { get; set; }
    }
}