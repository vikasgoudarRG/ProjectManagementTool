namespace ProjectManagementTool.Application.DTOs.Project
{
    public class FilterProjectDto
    {
        public string? TitleKeyword { get; set; }
        public Guid? ManagerId { get; set; }
        public string? Status { get; set; }
        public IEnumerable<Guid>? DeveloperIds { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public DateTime? CreatedAfter { get; set; }
    }
}