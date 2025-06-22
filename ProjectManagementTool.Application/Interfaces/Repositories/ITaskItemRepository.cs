using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITaskItemRepository
    {
        // TaskIteam Operations
        Task AddAsync(TaskItem taskItem);

        Task<TaskItem?> GetByIdAsync(Guid taskItemId);
        Task<IEnumerable<TaskItem>> GetAllByProjectId(Guid projectId); // for project leads
        Task<IEnumerable<TaskItem>> GetAllByTeamId(Guid teamId); // for project leads and team memebers
        Task<IEnumerable<TaskItem>> GetAllByAssignedUserId(Guid userId);
        Task<IEnumerable<TaskItem>> GetAllByProjectIdAndUserId(Guid projectId, Guid userId);
        Task<IEnumerable<TaskItem>> GetAllByTeamIdAndUserId(Guid teamId, Guid userId);

        Task UpdateAsync(TaskItem taskItem);

        Task DeleteAsync(TaskItem taskItem);
    }
}