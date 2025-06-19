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
                ProjectId = taskItem.ProjectId,
                ProjectTitle = taskItem.Project.Title,
                AssignedUser = taskItem.AssignedUserId,
                AssignedUsername = (taskItem.AssignedUser != null) ? taskItem.AssignedUser.Username : "",
                CreatedAt = taskItem.CreatedAt,
                Deadline = taskItem.Deadline,
                Tags = TagMapper.ToDtos(taskItem.Tags)
            };
        }

        public static ICollection<TaskItemDto> ToDtos(ICollection<TaskItem> taskItems)
        {
            ICollection<TaskItemDto> taskItemDtos = new List<TaskItemDto>();
            foreach (TaskItem taskItem in taskItems)
            {
                taskItemDtos.Add(ToDto(taskItem));
            }
            return taskItemDtos;
        }
    }
}