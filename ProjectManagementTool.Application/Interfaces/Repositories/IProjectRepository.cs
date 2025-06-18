using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<Project?> GetByIdAsync(Guid projectId);
        Task<ICollection<Project>> GetAllAsync();
        Task<ICollection<Project>> GetByUserIdAsync(Guid userId);

        Task SaveChangesAsync();
    }
}