using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task AddAsync(Comment comment);
        Task<ICollection<Comment>> GetByTaskIdAsync(Guid taskItemId);

        Task SaveChangesAsync();
    }
}