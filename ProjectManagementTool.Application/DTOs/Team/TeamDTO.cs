namespace ProjectManagementTool.Application.DTOs.Team
{
    public class TeamDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public IEnumerable<TeamMemberDTO> TeamMember { get; set; } = new List<TeamMemberDTO>();
        public DateTime CreatedOn { get; set; }
    }
}