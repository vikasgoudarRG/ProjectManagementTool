namespace ProjectManagementTool.Application.DTOs.Comment
{
    public class CreateCommentDto
    {
        public Guid TaskItemId { get; set; }
        public string Content { get; set; } = null!;
    }
}