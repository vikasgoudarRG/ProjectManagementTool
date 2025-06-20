using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Domain.Entities;

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
                Tags = (ICollection<TagDto>)taskItem.Tags.Select(t => TagMapper.ToDto(t))
            };
        }
    }
}