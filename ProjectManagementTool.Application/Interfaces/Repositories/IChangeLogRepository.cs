using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IChangeLogRepository
    {
        Task AddAsync(TaskItemChangeLog log);
        Task<ICollection<TaskItemChangeLog>> GetByTaskItemId(Guid taskItemId);

        Task SaveChangesAsync();
    }
}