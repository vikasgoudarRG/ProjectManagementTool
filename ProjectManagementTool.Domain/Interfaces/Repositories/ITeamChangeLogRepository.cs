using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Interfaces.Repositories
{
    public interface ITeamChangeLogRepository
    {
        Task AddAsync(TeamChangeLog log);

        Task<TeamChangeLog?> GetByIdAsync(Guid logId);

        Task<IEnumerable<TeamChangeLog>> GetAllByTeamIdAsync(
            Guid teamId,
            DateTime? from = null,
            DateTime? to = null,
            ChangeType? type = null
        );
    }
}
