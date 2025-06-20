using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.TaskItemChangeLog
{
    public class TaskItemChangeLogDto
    {
        public Guid Id { get; set; }
        public Guid TaskItemId { get; set; }
        public UserDto ChangedByUserDto { get; set; } = null!;
        public string ChangeType { get; set; } = null!;
        public string PropertyChanged { get; set; } = null!;
        public string OldValue { get; set; } = null!;
        public string NewValue { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }  
}