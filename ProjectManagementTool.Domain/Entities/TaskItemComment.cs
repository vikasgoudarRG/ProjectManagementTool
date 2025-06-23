namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItemComment
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid AuthorId { get; init; }
        public User Author { get; private set; } = null!;   

        public Guid TaskItemId { get; init; }
        public TaskItem TaskItem { get; private set; } = null!;

        private string _text = null!;
        public string Text
        {
            get => _text;
            private set => _text = ValidateText(value);
        }

        public DateTime CreatedOn { get; init; }

        public bool Edited { get; private set; }
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
        private static string ValidateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException(nameof(text), "Task Item Comment text cannot be null or whitespace");

            return text;
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
