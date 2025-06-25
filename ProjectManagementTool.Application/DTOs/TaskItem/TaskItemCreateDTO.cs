namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemCreateDTO
    {
        public Guid RequestorId { get; set; }
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Priority { get; set; } = null!;

        public string Status { get; set; } = null!;

        public Guid? AssignedUserId { get; set; }

        public Guid TeamId { get; set; }

        public DateTime? Deadline { get; set; }
    }
}