namespace ProjectManagementTool.Domain.Entities
{
    public class Comment
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid TaskItemId { get; private set; }
        public TaskItem TaskItem { get; private set; } = null!;

        public Guid AuthorId { get; private set; }
        public User Author { get; private set; } = null!;

        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; init; }
        #endregion Fields

        #region Constructors
        private Comment() { }

        public Comment(Guid taskItemId, Guid authorId, string content)
        {
            Id = Guid.NewGuid();
            TaskItemId = taskItemId;
            AuthorId = authorId;
            Content = content;
            CreatedOn = DateTime.UtcNow;
        }

        #endregion Constructors
    }
}
