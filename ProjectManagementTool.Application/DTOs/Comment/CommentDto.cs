using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.Comment
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid TaskItemId { get; set; }
        public string Content { get; set; } = null!;
        public UserDto Author { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}