namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class AssignTaskItemDTO
    {
        public Guid TaskItemId { get; set; }

        public Guid AssignedUserId { get; set; }

        public Guid RequesterId { get; set; }
    }
}
