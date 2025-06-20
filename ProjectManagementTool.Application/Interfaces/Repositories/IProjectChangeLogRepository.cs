using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IProjectChangeLogRepository
    {
        Task AddAsync(ProjectChangeLog log);
        Task<ProjectChangeLog?> GetById(Guid id);
        Task<IEnumerable<ProjectChangeLog>> GetAllByProjectId(Guid project);
        Task UpdateAsync(ProjectChangeLog projectChangeLog);
        Task DeleteAsync(ProjectChangeLog projectChangeLog);
        Task SaveChangesAsync();
    }
}