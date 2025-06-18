namespace ProjectManagementTool.Application.DTOs.Project
{
    public class AssignProjectDevelopers
    {
        public Guid ProjectId { get; set; }
        public ICollection<Guid> DeveloperIds { get; set; } = new List<Guid>();
    }
}