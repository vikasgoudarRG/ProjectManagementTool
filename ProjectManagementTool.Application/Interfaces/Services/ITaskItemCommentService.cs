using ProjectManagementTool.Application.DTOs.Comment;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemCommentService
    {
        Task AddAsync(CreateCommentDTO dto);
        Task UpdateAsync(Guid commentId, Guid authorId, string updatedContent);
        Task DeleteAsync(Guid commentId, Guid authorId);

        Task<IEnumerable<TaskItemCommentDTO>> GetAllForTaskAsync(Guid taskId);
    }

}