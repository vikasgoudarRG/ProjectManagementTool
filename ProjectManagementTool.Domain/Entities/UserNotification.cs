namespace ProjectManagementTool.Domain.Entities
{
    public class UserNotification
    {
        #region Fields
        public Guid Id { get; init; }
        public Guid UserId { get; init; }

        private string _message = null!;
        public string Message
        {
            get => _message;
            private set => _message = ValidateAndGetMessage(value);
        }
        public bool IsRead { get; private set; }
        public DateTime CreatedOn { get; init; }
        public DateTime? ReadOn { get; private set; }
        #endregion Fields

        #region Constructors
        private UserNotification() { }

        public UserNotification(Guid userId, string message)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Message = message;
            IsRead = false;
            CreatedOn = DateTime.UtcNow;
        }
        #endregion Constructors

        #region Methods
        // =============== static methods ===============
        private static string ValidateAndGetMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be null or whitespace", nameof(message));
            return message;
        }

        // =============== methods ===============
        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadOn = DateTime.UtcNow;
            }
        }
        #endregion Methods
    }
}