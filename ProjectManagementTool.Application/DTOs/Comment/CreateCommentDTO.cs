namespace ProjectManagementTool.Application.DTOs.Comment
{
    public class CreateCommentDTO
    {
        public Guid TaskItemId { get; set; }

        public Guid AuthorId { get; set; }

        public string Text { get; set; } = null!;
    }
}