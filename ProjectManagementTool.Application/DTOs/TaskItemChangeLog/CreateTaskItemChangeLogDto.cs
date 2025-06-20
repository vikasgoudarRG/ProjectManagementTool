namespace ProjectManagementTool.Application.DTOs
{
    public class CreateTaskItemChangeLogDto
    {
        public Guid TaskItemId { get; set; }
        public Guid ChangedByUserId { get; set; }
        public string ChangeType { get; set; } = null!;
        public string PropertyChanged { get; set; } = null!;
        public string OldValue { get; set; } = null!;
        public string NewValue { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}