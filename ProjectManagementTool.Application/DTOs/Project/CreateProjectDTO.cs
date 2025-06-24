namespace ProjectManagementTool.Application.DTOs.Project
{
    public class CreateProjectDTO
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid ProjectLeadId { get; set; }
    }
}