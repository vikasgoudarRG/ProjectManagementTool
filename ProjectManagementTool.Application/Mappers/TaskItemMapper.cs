using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public class TaskItemMapper : ITaskItemMapper
    {
        public TaskItemDTO ToDTO(TaskItem taskItem)
        {
            return new TaskItemDTO
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Type = taskItem.Type.ToString(),
                Priority = taskItem.Priority.ToString(),
                Status = taskItem.Status.ToString(),
                AssignedUserId = taskItem.AssignedUserId,
                TeamId = taskItem.TeamId,
                CreatedAt = taskItem.CreatedAt,
                Deadline = taskItem.Deadline,
                CompletedAt = taskItem.CompletedAt
            };
        }
    }
}