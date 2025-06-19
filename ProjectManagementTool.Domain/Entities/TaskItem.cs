using System.Reflection.Metadata;
using ProjectManagementTool.Domain.Enums.TaskItem;
namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; init; }

        private string _title = null!;
        public string Title
        {
            get => _title;
            set => _title = IsValidTitle(value) ? value : throw new Exception($"TaskItem Title - {value} is invalid");
        }

        private string _description = null!;
        public string Description
        {
            get => _description;
            set => _description = IsValidDescription(value) ? value : throw new Exception($"TaskItem Description - {value} is invalid");
        }
        public TaskItemType Type { get; set; }
        public TaskItemPriority Priority { get; set; }
        public TaskItemStatus Status { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public DateTime CreatedAt { get; init; }
        public DateTime? Deadline { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<TaskItemChangeLog> ChangeLogs { get; set; } = new List<TaskItemChangeLog>();

        private TaskItem() { }

        public TaskItem(string title, string description, TaskItemType type, TaskItemPriority priority, TaskItemStatus status, Guid projectId, Guid? assignedUserId, DateTime? deadline, IEnumerable<Tag>? tags = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Type = type;
            Priority = priority;
            Status = status;
            ProjectId = projectId;
            AssignedUserId = assignedUserId;
            CreatedAt = DateTime.UtcNow;
            Deadline = deadline;
            if (tags != null)
            {
                Tags = tags.ToList<Tag>();
            }
        }   

        private static bool IsValidTitle(string title)
        {
            return string.IsNullOrWhiteSpace(title) ? false : true;
        }

        private static bool IsValidDescription(string description)
        {
            return string.IsNullOrWhiteSpace(description) ? false : true;
        }
    }
}