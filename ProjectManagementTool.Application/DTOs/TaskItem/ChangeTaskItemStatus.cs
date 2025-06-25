namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class ChangeTaskItemStatusDTO
    {
        public string NewStatus { get; set; } = null!;

        public Guid RequesterId { get; set; }
    }
}
