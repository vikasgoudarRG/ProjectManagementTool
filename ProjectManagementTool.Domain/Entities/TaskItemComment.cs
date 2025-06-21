namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItemComment
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid AuthorId { get; private set; }
        public User Author { get; private set; } = null!;

        public Guid TaskItemId { get; private set; }
        public TaskItem TaskItem { get; private set; } = null!;

        private string _text = null!;
        public string Text
        {
            get => _text;
            set => _text = IsValidCommentText(value) ? value : throw new Exception($"Invalid Comment text: {value}");
        }

        public DateTime CreatedOn { get; init; }

        public bool Edited { get; set; }
        public DateTime? LastEditedOn { get; private set; }
        #endregion Fields

        #region Constructors
        private TaskItemComment() { }

        public TaskItemComment(Guid taskItemId, Guid authorId, string text)
        {
            Id = Guid.NewGuid();
            AuthorId = authorId;
            TaskItemId = taskItemId;
            Text = text;
            CreatedOn = DateTime.UtcNow;
            Edited = false;
        }
        #endregion Constructors

        #region Methods
        private static bool IsValidCommentText(string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
        public void Edit(string newText)
        {
            if (newText == Text) return;
            Text = newText;
            Edited = true;
            LastEditedOn = DateTime.UtcNow;
        }
        #endregion Methods
    }
}
