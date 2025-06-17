using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetAllAsync(Guid id);
    }
}