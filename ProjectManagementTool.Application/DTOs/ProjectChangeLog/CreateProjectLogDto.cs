namespace ProjectManagementTool.Application.DTOs.ProjectChangeLog
{
    public class CreateProjectChangeLogDto
    {
        public Guid ProjectId { get; set; }
        public Guid ChangedByUserId { get; set; }
        public string ChangeType { get; set; } = null!;
        public string PropertyChanged { get; private set; } = null!;
        public string OldValue { get; private set; } = null!;
        public string NewValue { get; private set; } = null!;
        public DateTime CreatedOn { get; init; }
    }
}