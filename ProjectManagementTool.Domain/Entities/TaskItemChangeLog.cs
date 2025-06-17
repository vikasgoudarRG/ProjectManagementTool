namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItemChangeLog
    {
        public Guid Id { get; set; }

        public Guid TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; } = null!;

        public Guid ChangedByUserId { get; set; }
        public User ChangedByUser { get; set; } = null!;

        public string PropertyChanged { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}