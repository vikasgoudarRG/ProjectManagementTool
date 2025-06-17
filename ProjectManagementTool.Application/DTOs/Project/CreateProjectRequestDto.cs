namespace ProjectManagementTool.Application.DTOs.Project
{
    public class CreateProjectRequestDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ManagerId { get; set; }
    }
}