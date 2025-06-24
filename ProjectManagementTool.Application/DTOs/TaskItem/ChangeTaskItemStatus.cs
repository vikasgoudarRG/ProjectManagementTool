namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class ChangeTaskItemStatusDTO
    {
        public Guid TaskItemId { get; set; }

        public string NewStatus { get; set; } = null!;

        public Guid RequesterId { get; set; }
    }
}
