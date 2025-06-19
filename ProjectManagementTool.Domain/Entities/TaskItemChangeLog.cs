namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItemChangeLog
    {
        public Guid Id { get; init; }

        public Guid TaskItemId { get; private set; }
        public TaskItem TaskItem { get; private set; } = null!;

        public Guid ChangedByUserId { get; private set; }
        public User ChangedByUser { get; private set; } = null!;

        public string PropertyChanged { get; private set; } = null!;
        public string? OldValue { get; private set; }
        public string? NewValue { get; private set; }
        public DateTime ChangedAt { get; init; }

        private TaskItemChangeLog() { }

        public TaskItemChangeLog(Guid taskItemId, Guid changedByUserId, string propertyChanged, string? oldValue, string? newValue)
        {
            Id = Guid.NewGuid();
            TaskItemId = taskItemId;
            ChangedByUserId = changedByUserId;
            PropertyChanged = propertyChanged;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedAt = DateTime.UtcNow;
        }
    }
}