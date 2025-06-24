using ProjectManagementTool.Domain.Enums.Team;

namespace ProjectManagementTool.Application.DTOs.Team
{
    public class TeamMemberActionDTO
    {
        public Guid TeamId { get; set; }

        public Guid UserId { get; set; }

        public Guid RequesterId { get; set; }

        public string Role { get; set; } = null!;
    }
}
