using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task AddAsync(Comment comment);

        Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskItemId);
        Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId);
        
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
        

        Task SaveChangesAsync();
    }
}