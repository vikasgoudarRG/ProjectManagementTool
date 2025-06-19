using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task AddAsync(string name);
        Task<ICollection<Tag>> GetAllAsync();
        Task<Tag?> GetByNameAsync(string name);
        Task<Tag> GetOrCreateAsync(string name);

        Task SaveChangesAsync();
    }
}