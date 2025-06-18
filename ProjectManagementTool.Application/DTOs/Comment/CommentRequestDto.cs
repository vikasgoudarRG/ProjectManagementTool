namespace ProjectManagementTool.Application.DTOs.Comment
{
    public class CommentRequestDto
    {
        public Guid TaskItemId { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; } = null!;
    }
}