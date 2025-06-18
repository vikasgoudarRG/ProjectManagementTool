namespace ProjectManagementTool.Application.DTOs.Project
{
    public class UpdateProjectRequestDto
    {  
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
        public string? Status { get; set; }
    }
}