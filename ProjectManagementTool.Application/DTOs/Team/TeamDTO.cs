namespace ProjectManagementTool.Application.DTOs.Team
{
    public class TeamDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}