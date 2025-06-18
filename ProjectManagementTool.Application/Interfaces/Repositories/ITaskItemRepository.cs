using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITaskItemRepository
    {
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(TaskItem taskItem);
        Task<TaskItem?> GetByIdAsync(Guid taskItemId);
        Task<ICollection<TaskItem>> GetAllByProjectId(Guid projectId);

        Task SaveChangesAsync();
    }
}