namespace ProjectManagementTool.Application.DTOs.TaskItem {
    public class UpdateTaskItemDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? Type { get; set; }
        public Guid? AssignedUserId { get; set; }
        public DateTime? Deadline { get; set; }
    }
}