using ProjectManagementTool.Domain.Enums.TaskItem;
namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskItemType Type { get; set; }
        public TaskItemPriority Priority { get; set; }
        public TaskItemStatus Status { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<TaskItemChangeLog> ChangeLogs { get; set; } = new List<TaskItemChangeLog>();
    }
}