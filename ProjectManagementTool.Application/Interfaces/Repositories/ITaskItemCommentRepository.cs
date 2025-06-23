using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface ITaskItemCommentRepository
    {
        Task AddAsync(TaskItemComment comment);

        Task<TaskItemComment?> GetByIdAsync(Guid commentId);
        Task<IEnumerable<TaskItemComment>> GetAllByTaskIdAsync(Guid taskItemId);
        Task<IEnumerable<TaskItemComment>> GetAllByUserIdAsync(Guid userId);

        Task UpdateAsync(TaskItemComment comment);

        Task DeleteAsync(TaskItemComment comment);
    }
}