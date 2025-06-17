namespace ProjectManagementTool.Application.DTOs.Comment
{
    public class CommentDto
    {
        public string AuthorName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}