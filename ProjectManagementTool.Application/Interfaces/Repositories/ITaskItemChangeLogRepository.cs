using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITaskItemChangeLogRepository
    {
        Task AddAsync(TaskItemChangeLog log);

        Task<TaskItemChangeLog?> GetById(Guid id);
        Task<IEnumerable<TaskItemChangeLog>> GetAllByTaskItemId(Guid taskItemId);
    }
}