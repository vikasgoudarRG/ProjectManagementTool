namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemFilterRequestDto
    {
        public Guid? Id { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string? AssignedUsername { get; set; }
        public string? Type { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public ICollection<string>? Tags = new List<string>();
        public DateTime? DeadlineBefore { get; set; }
        public DateTime? DeadlineAfter { get; set; }
    }
}