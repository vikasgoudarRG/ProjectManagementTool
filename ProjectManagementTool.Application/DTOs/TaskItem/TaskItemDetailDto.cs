using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;
using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemDetailDto : TaskItemSummaryDto
    {
        public string Description { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public string? AssignedUsername { get; set; }

        public DateTime CreatedOn { get; set; }
        public ICollection<string> Tags = new List<string>();
    }
}