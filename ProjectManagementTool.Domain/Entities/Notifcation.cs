namespace ProjectManagementTool.Domain.Entities
{
    public class Notification
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        private string _message = null!;
        public string Message
        {
            get => _message;
            private set => _message = IsValidMessage(value) ? value : throw new Exception($"Notification Message - {value} is invalid");
        }

        public Guid ProjectId { get; set; }
        public Guid TaskItemId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; init; }
        #endregion Fields

        #region Constructors
        private Notification() { }

        public Notification(Guid userId, string message, Guid projectId, Guid taskItemId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Message = message;
            IsRead = false;
            CreatedOn = DateTime.UtcNow;
            ProjectId = projectId;
            TaskItemId = taskItemId;
        }
        #endregion Constructors

        #region Methods
        private static bool IsValidMessage(string message)
        {
            return string.IsNullOrWhiteSpace(message) ? false : true;
        }
        #endregion Methods
    }
}