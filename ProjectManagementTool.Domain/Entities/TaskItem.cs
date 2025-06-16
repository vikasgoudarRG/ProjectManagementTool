using ProjectManagementTool.Domain.Enums;
namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskType Type { get; set; }
        public TaskPriority Priority { get; set; }
        public ProjectManagementTool.Domain.Enums.TaskStatus Status { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }

        public ICollection<Comment> Comments = new List<Comment>();
        public ICollection<Tag> Tags = new List<Tag>();
        public ICollection<TaskChangeLog> ChangeLogs = new List<TaskChangeLog>();
    }
}