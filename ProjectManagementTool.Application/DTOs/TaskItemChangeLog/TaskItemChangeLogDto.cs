namespace ProjectManagementTool.Application.DTOs.TaskItemChangeLog
{
    public class TaskItemChangeLogDto
    {
        public string TaskItemTitle { get; set; } = null!;
        public Guid TaskItemId { get; set; }
        public string ChangedByUsername { get; set; } = null!;
        public string PropertyChanged { get; set; } = null!;
        public string OldValue { get; set; } = null!;
        public string NewValue { get; set; } = null!;
        public DateTime ChangedAt { get; set; }
    }  
}