namespace ProjectManagementTool.Application.DTOs.TaskItemChangeLog
{
    public class TaskItemChangeLogDto
    {
        public string TaskItemTitle { get; set; } = null!;
        public Guid TaskItemId { get; set; }
        public string ChangedByUsername { get; set; } = null!;
        public string PropertyChanged { get; set; } = null!;
        public string OldValue = null!;
        public string NewValue = null!;
        public DateTime ChangedAt { get; set; }
    }  
}