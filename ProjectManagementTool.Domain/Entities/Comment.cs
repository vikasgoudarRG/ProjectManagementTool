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

        private string _content = null!;
        public string Content
        {
            get => _content;
            set
            {
                if (!IsValidComment(value))
                {
                    throw new Exception($"Invalid Comment message: {value}");
                }
                
                _content = value;
                if (!Edited)
                {
                    Edited = true;
                    LastEditedOn = DateTime.UtcNow;
                }
            }
        }

        public bool Edited { get; private set; } = false;
        public DateTime? LastEditedOn { get; private set; }
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

        #region Methods
        private static bool IsValidComment(string comment)
        {
            return !string.IsNullOrWhiteSpace(comment);
        }
        #endregion Methods
    }
}
