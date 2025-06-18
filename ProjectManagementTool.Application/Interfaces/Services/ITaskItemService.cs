using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemService
    {
        Task<Guid> CreateTaskItemAsync(CreateTaskItemRequestDto dto);
        Task UpdateTaskItemAsync(UpdateTaskItemRequestDto dto);
        Task DeleteTaskItemAsync(Guid taskItem);
        Task<TaskItem?> GetTaskItemById(Guid taskItemId);
        Task<ICollection<TaskItem>> GetAllTaskItemsByProjectId(Guid projectId);
        Task<ICollection<TaskItem>> GetAllTaskItemsByFiler(TaskItemFilterRequestDto dto);
        Task<ICollection<TaskItemChangeLogDto>> GetChangeLogForTaskItem(Guid taskItemId);
    }
}