using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag);
        Task AddManyAsync(ICollection<Tag> tags);
        Task<ICollection<Tag>> GetAllAsync();
        Task<Tag?> GetByNameAsync(string name);
        Task<Tag> GetOrCreateAsync(string name);
        Task<ICollection<Tag>> GetOrCreateManyAsync(ICollection<string> names);

        Task SaveChangesAsync();
    }
}