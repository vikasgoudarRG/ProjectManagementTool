namespace ProjectManagementTool.Domain.Entities
{
    public class UserNotification
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid UserId { get; private set; }

        private string _message = null!;
        public string Message
        {
            get => _message;
            private set => _message = IsValidMessage(value) ? value : throw new Exception($"Invalid Notification message: {value}");
        }
        public bool IsRead { get; private set; }
        public DateTime CreatedOn { get; init; }
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
        private static bool IsValidMessage(string message)
        {
            return !string.IsNullOrWhiteSpace(message);
        }

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
            }
        }
        #endregion Methods
    }
}