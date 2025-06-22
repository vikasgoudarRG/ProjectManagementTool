using ProjectManagementTool.Domain.Entities;

namespace ProjectManageMentTool.Application.Interfaces.Repositories
{
    public interface ITeamChangeLogRepository
    {
        Task AddAsync(TeamChangeLog log);

        Task<TeamChangeLog?> GetByIdAsync(Guid logId);
        Task<IEnumerable<TeamChangeLog>> GetAllByTeamIdAsync(Guid teamId);
    }
}