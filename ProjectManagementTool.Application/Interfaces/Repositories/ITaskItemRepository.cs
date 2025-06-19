using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITaskItemRepository
    {
        Task AddAsync(TaskItem taskItem);

        Task<TaskItem?> GetByIdAsync(Guid taskItemId);
        Task<IEnumerable<TaskItem>> GetAllByProjectId(Guid projectId);
        Task<IEnumerable<TaskItem>> GetAllTaskItemsByFilter(TaskItemFilterQueryModel queryModel);

        Task UpdateAsync(TaskItem taskItem);
        
        Task DeleteAsync(TaskItem taskItem);
        

        Task SaveChangesAsync();
    }
}