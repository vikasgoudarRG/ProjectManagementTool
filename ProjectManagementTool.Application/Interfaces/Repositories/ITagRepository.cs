using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag);

        Task AddManyAsync(IEnumerable<Tag> tags);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByNameAsync(string name);
        // Task<Tag> GetOrCreateAsync(string name);
        // Task<IEnumerable<Tag>> GetOrCreateManyAsync(IEnumerable<string> names);

        Task UpdateAsync(Tag tag);

        Task DeleteAsync(Tag tag);

        Task SaveChangesAsync();
    }
}