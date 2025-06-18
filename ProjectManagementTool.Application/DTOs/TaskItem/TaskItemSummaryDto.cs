namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}