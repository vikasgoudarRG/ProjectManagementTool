using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemService
    {
        Task<TaskItemDto> CreateTaskItemAsync(Guid creatorId, CreateTaskItemDto dto);
        Task<TaskItemDto> GetTaskItemById(Guid taskItemId);
        Task<IEnumerable<TaskItemDto>> GetAllTaskItemsByProject(Guid projectId);
        Task<IEnumerable<TaskItemDto>> GetAllTaskItemsByFilter(FilterTaskItemDto dto);
        Task<IEnumerable<TaskItemChangeLogDto>> GetTaskItemChangeHistoryAsync(Guid taskItemId);
        
        Task<IEnumerable<Comment>> GetComments(Guid taskItemId);
        Task UpdateTaskItemAsync(Guid taskItemId, UpdateTaskItemDto dto);
        Task DeleteTaskItemAsync(Guid taskItemId);
        
    }
}