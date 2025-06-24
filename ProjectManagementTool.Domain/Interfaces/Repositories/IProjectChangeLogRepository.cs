using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Interfaces.Repositories
{
    public interface IProjectChangeLogRepository
    {
        Task AddAsync(ProjectChangeLog log);

        Task<ProjectChangeLog?> GetByIdAsync(Guid logId);
        Task<IEnumerable<ProjectChangeLog>> GetAllByProjectIdAsync(
            Guid projectId,
            DateTime? from = null,
            DateTime? to = null,
            ChangeType? type = null
        );
    }
}