namespace ProjectManagementTool.Application.DTOs.ChangeLog
{
    public abstract class ChangeLogBaseDto
    {
        public Guid Id { get; set; }
        public Guid ChangedByUserId { get; set; }
        public string PropertyChanged { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string ChangeType { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}