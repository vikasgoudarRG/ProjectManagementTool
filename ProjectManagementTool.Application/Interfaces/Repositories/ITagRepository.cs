using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag);
        Task<ICollection<Tag>> GetAllAsync();
        Task<Tag?> GetByNameAsync(string name);

        Task SaveChangesAsync();
    }
}