namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class UpdateTaskItemRequestDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }

        public Guid? AsigneeId { get; set; }

        public DateTime? Deadline { get; set; }

        public ICollection<string>? Tags { get; set; }
    }
}