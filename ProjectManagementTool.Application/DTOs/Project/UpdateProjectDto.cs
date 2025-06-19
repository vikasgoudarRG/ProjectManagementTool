namespace ProjectManagementTool.Application.DTOs.Project
{
    public class UpdateProjectDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public ICollection<Guid>? DeveloperIds { get; set; }
    }
}