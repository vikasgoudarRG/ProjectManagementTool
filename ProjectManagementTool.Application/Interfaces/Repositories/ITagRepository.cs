using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag);
        Task AddRangeAsync(IEnumerable<Tag> tags);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByNameAsync(string name);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(Tag tag);
        Task DeleteRangeAsync(IEnumerable<Tag> tags);
        Task SaveChangesAsync();
    }
}