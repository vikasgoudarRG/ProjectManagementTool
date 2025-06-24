namespace ProjectManagementTool.Application.DTOs.Project
{
    public class UpdateProjectDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}