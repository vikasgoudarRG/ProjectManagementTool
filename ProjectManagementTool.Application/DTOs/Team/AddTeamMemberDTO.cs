namespace ProjectManagementTool.Application.DTOs.Team
{
    public class AddTeamMemberDTO
    {
        public Guid TeamId { get; set; }

        public Guid UserId { get; set; }

        public string Role { get; set; } = null!;
    }
}