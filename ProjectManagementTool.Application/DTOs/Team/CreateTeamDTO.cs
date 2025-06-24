namespace ProjectManagementTool.Application.DTOs.Team
{
    public class CreateTeamDTO
    {
        public string Name { get; set; } = null!;

        public Guid ProjectId { get; set; }
    }
}