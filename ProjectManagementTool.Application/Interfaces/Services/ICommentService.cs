using ProjectManagementTool.Application.DTOs.Comment;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemCommentService
    {
        Task AddCommentAsync(Guid taskId, Guid authorId, string content);
        Task<IEnumerable<TaskItemCommentDTO>> GetCommentsForTaskAsync(Guid taskId);
    }
}