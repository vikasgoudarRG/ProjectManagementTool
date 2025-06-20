using System.Net;
using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.TaskItem;

namespace ProjectManagementTool.Application.Mappers
{
    public static class TaskItemMapper
    {
        public static TaskItemDto ToDto(TaskItem taskItem)
        {
            return new TaskItemDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Type = taskItem.Type.ToString(),
                Priority = taskItem.Priority.ToString(),
                Status = taskItem.Status.ToString(),
                ProjectId = taskItem.ProjectId,
                ProjectName = taskItem.Project.Title,
                AssignedUserId = taskItem.AssignedUserId,
                AssignedUsername = taskItem.AssignedUser != null ? taskItem.AssignedUser.Username : null,
                CreatedAt = taskItem.CreatedAt,
                Deadline = taskItem.Deadline,
                Tags = taskItem.Tags.Select(t => TagMapper.ToDto(t))
            };
        }

        public static TaskItemFilterQueryModel ToFilterQueryModel(FilterTaskItemDto dto)
        {
            return new TaskItemFilterQueryModel
            {
                ProjectId = dto.ProjectId,
                TitleKeyword = dto.TitleKeyword,
                AssignedUserId = dto.AssignedUserId,
                Type = Enum.TryParse<TaskItemType>(dto.Type, ignoreCase: true, out TaskItemType type) ? type : throw new Exception($"Type {dto.Type} is invalid"),
                Priority = Enum.TryParse<TaskItemPriority>(dto.Priority, ignoreCase: true, out TaskItemPriority priority) ? priority : throw new Exception($"Type {dto.Priority} is invalid"),
                Status = Enum.TryParse<TaskItemStatus>(dto.Status, ignoreCase: true, out TaskItemStatus status) ? status : throw new Exception($"Type {dto.Status} is invalid"),
                TagIds = dto.TagIds,
                DeadlineBefore = dto.DeadlineBefore,
                DeadlineAfter = dto.DeadlineAfter
            };
        }
    }
}