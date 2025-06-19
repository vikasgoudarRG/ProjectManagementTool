using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.TaskItem;

namespace ProjectManagementTool.Application.QueryModels
{
    public class TaskItemFilterQM
    {
        public Guid? ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public TaskItemType? Type { get; set; }
        public TaskItemPriority? Priority { get; set; }
        public TaskItemStatus? Status { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public DateTime? DeadlineBefore { get; set; }
        public DateTime? DeadlineAfter { get; set; }
    }
}