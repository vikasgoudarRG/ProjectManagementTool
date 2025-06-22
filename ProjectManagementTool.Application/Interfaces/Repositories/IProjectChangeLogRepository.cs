using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IProjectChangeLogRepository
    {
        Task AddAsync(ProjectChangeLog log);

        Task<ProjectChangeLog?> GetByIdAsync(Guid logId);
        Task<IEnumerable<ProjectChangeLog>> GetAllByProjectIdAsync(Guid projectId);
    }
}