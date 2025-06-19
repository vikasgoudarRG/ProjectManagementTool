using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.TaskItem;

namespace ProjectManagementTool.Application.QueryModels
{
    public class TaskItemFilterQueryModel
    {
        public Guid? ProjectId { get; set; }
        public string? TitleKeyword { get; set; }
        public Guid? AssignedUserId { get; set; }
        public TaskItemType? Type { get; set; }
        public TaskItemPriority? Priority { get; set; }
        public TaskItemStatus? Status { get; set; }
        public IEnumerable<Guid>? TagIds { get; set; }
        public DateTime? DeadlineBefore { get; set; }
        public DateTime? DeadlineAfter { get; set; }
    }
}