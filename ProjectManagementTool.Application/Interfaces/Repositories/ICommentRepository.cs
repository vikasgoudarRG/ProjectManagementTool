using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task AddAsync(Comment comment);

        Task<IEnumerable<Comment>> GetAllByTaskIdAsync(Guid taskItemId);
        Task<IEnumerable<Comment>> GetAllByUserIdAsync(Guid userId);
        
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
        

        Task SaveChangesAsync();
    }
}