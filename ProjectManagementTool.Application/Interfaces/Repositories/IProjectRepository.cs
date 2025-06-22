using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project);
        
        Task<Project?> GetByIdAsync(Guid projectId);
        Task<Project?> GetByNameAsync(string projectName);
        Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId);
        Task<IEnumerable<Project>> GetAllByFilterAsync(ProjectFilterQueryModel filterQuery);

        Task UpdateAsync(Project project);

        Task DeleteAsync(Project project);
    }
}