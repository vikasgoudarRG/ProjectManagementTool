using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;

namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemDetailDto : TaskItemDto
    {
        public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public ICollection<TaskItemChangeLogDto> ChangeLogs { get; set; } = new List<TaskItemChangeLogDto>();
    }
}