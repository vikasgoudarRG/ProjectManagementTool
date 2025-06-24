namespace ProjectManagementTool.Application.DTOs.Team
{
    public class CreateTeamDTO
    {
        public string Name { get; set; } = null!;
        public Guid RequesterId { get; set; }
        public Guid ProjectId { get; set; }
    }
}