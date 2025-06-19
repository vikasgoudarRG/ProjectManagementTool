namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class CreateTaskItemRequestDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public DateTime? Deadline { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
    }
}