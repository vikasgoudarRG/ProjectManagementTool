using System.Reflection.Metadata;
using ProjectManagementTool.Domain.Enums.TaskItem;
namespace ProjectManagementTool.Domain.Entities
{
    public class TaskItem
    {
        #region Fields
        public Guid Id { get; init; }

        private string _title = null!;
        public string Title
        {
            get => _title;
            set => _title = ValidateTitle(value);
        }

        private string _description = null!;
        public string Description
        {
            get => _description;
            set => _description = ValidateDescription(value);
        }
        public TaskItemType Type { get; set; }
        public TaskItemPriority Priority { get; set; }
        public TaskItemStatus Status { get; set; }

        public Guid ProjectId { get; init; }
        public Project Project { get; init; } = null!;

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public DateTime CreatedAt { get; init; }
        public DateTime? Deadline { get; set; }
        public DateTime? CompletedAt { get; set; }

        private readonly List<TaskItemComment> _comments = new List<TaskItemComment>();
        public IReadOnlyCollection<TaskItemComment> Comments => _comments.AsReadOnly();

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        #endregion Fields

        #region Constructors        
        private TaskItem() { }

        public TaskItem(string title, string description, TaskItemType type, TaskItemPriority priority, TaskItemStatus status, Guid projectId, Guid? assignedUserId, DateTime? deadline)
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
        }
        #endregion Constructors   

        #region Methods
        private static string ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(nameof(title), "TaskItem TItle cannot be null or empty");
            }
            return title;
        }

        private static string ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(nameof(description), "TaskItem Description cannot be null or empty");
            }
            return description;
        }

        public void AddComment(TaskItemComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment), "Comment cannot be null");
            if (comment.TaskItemId != Id) throw new InvalidOperationException(nameof(comment) + "Comment does not belong to this TaskItem");
            if (!_comments.Any(c => c.Id == comment.Id))
            {
                _comments.Add(comment);
            }
        }
        public void RemoveComment(TaskItemComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment), "Comment cannot be null");
            if (comment.TaskItemId != Id) throw new InvalidOperationException(nameof(comment) + "Comment does not belong to this TaskItem");
            if (_comments.Any(c => c.Id == comment.Id))
            {
                _comments.Remove(comment);
            }
        }
        #endregion Methods
    }
}