using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IChangeLogRepository
    {
        Task AddAsync(TaskItemChangeLog log);
        Task<IEnumerable<TaskItemChangeLog>> GetAllByTaskItemId(Guid taskItemId);
        Task UpdateAsync(TaskItemChangeLog taskItemChangeLog);
        Task DeleteAsync(TaskItemChangeLog taskItemChangelog);

        Task SaveChangesAsync();
    }
}