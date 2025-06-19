namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemFilterRequestDto
    {
        public Guid? ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string? Type { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public ICollection<string>? Tags { get; set; } 
        public DateTime? DeadlineBefore { get; set; }
        public DateTime? DeadlineAfter { get; set; }
    }
}