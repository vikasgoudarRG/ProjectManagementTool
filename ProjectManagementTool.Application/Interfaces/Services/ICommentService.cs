using ProjectManagementTool.Application.DTOs.Comment;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentRequestDto dto);
        Task<ICollection<CommentDto>> GetCommentsByTaskIdAsync(Guid taskId);
    }
}