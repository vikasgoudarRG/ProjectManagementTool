using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Interfaces.Repositories
{
    public interface ITaskItemChangeLogRepository
    {
        Task AddAsync(TaskItemChangeLog log);

        Task<TaskItemChangeLog?> GetByIdAsync(Guid id);

        Task<IEnumerable<TaskItemChangeLog>> GetAllByTaskItemIdAsync(
            Guid taskItemId,
            DateTime? from = null,
            DateTime? to = null,
            ChangeType? type = null
        );
    }
}
