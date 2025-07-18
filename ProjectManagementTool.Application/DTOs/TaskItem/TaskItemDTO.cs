namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid? AssignedUserId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}