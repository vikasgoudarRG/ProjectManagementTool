using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Domain.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        
        Task<Project?> GetByIdAsync(Guid projectId);
        Task<Project?> GetByNameAsync(string projectName);
        Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId);

        Task UpdateAsync(Project project);

        Task DeleteAsync(Project project);
    }
}