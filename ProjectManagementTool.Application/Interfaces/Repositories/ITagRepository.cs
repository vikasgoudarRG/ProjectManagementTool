using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> GetOrCreateAsync(string name);

        Task<Tag?> GetByIdAsync(Guid tagId);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByNameAsync(string name);

        Task DeleteAsync(Tag tag);
    }
}
