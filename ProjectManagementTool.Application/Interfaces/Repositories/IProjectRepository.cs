using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);

        Task<Project?> GetByIdAsync(Guid projectId);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId);

        
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        

        Task SaveChangesAsync();
    }
}