namespace ProjectManagementTool.Application.DTOs.Comment
{
    public class TaskItemCommentDTO
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Text { get; set; } = null!;
        public bool Edited { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastEditedOn { get; set; }
    }
}