namespace ProjectManagementTool.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        // ManagerId is set from the authenticated user (creator)

        public ICollection<Guid>? DeveloperIds { get; set; }
    }
}