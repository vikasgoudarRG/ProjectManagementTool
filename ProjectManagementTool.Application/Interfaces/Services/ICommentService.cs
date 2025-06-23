using ProjectManagementTool.Application.DTOs.Comment;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemCommentService
    {
        Task AddCommentAsync(Guid taskId, Guid authorId, string content);
        Task UpdateCommentAsync(Guid commentId, Guid authorId, string updatedContent);
        Task DeleteCommentAsync(Guid commentId, Guid authorId);
        Task<IEnumerable<TaskItemCommentDTO>> GetCommentsForTaskAsync(Guid taskId);
    }
}