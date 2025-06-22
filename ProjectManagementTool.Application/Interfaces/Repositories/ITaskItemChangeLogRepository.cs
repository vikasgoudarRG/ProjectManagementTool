using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITaskItemChangeLogRepository
    {
        Task AddAsync(TaskItemChangeLog log);

        Task<TaskItemChangeLog?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskItemChangeLog>> GetAllByTaskItemIdAsync(Guid taskItemId);
    }
}