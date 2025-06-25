using ProjectManagementTool.Domain.Entities.ChangeLogs;
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

        public Guid TeamId { get; init; }
        public Team Team { get; private set; } = null!;

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public DateTime CreatedAt { get; init; }
        public DateTime? Deadline { get; set; }
        public DateTime? CompletedAt { get; set; }

        public ICollection<TaskItemComment> Comments { get; private set; } = new List<TaskItemComment>();
        public ICollection<Tag> Tags { get; private set; } = new List<Tag>();
        public ICollection<TaskItemChangeLog> ChangeLogs { get; private set; } = new List<TaskItemChangeLog>();
        #endregion Fields

        #region Constructors        
        private TaskItem() { }

        public TaskItem(string title, string description, TaskItemType type, TaskItemPriority priority, TaskItemStatus status, Guid? assignedUserId, DateTime? deadline)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Type = type;
            Priority = priority;
            Status = status;
            AssignedUserId = assignedUserId;
            CreatedAt = DateTime.UtcNow;
            Deadline = deadline;
        }
        #endregion Constructors   

        #region Methods
        // =============== static methods ===============
        private static string ValidateTitle(string title)
        {
            title = title.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
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


        // =============== methods ===============
        public void AddComment(TaskItemComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (comment.TaskItemId != Id) throw new InvalidOperationException("Comment does not belong to this TaskItem");
            if (!Comments.Any(c => c.Id == comment.Id))
            {
                Comments.Add(comment);
            }
        }

        public void Edit(Guid commentId, string text)
        {
            TaskItemComment? comment = Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null) throw new InvalidOperationException("Comment does not belong to this TaskItem");
            comment.Edit(text);
        }

        public void RemoveComment(TaskItemComment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment), "Comment cannot be null");
            if (comment.TaskItemId != Id) throw new InvalidOperationException("Comment does not belong to this TaskItem");
            if (Comments.Any(c => c.Id == comment.Id))
            {
                Comments.Remove(comment);
            }
        }

        public void AddTag(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (!Tags.Any(t => t.Id == tag.Id)) Tags.Add(tag);
        }

        public void RemoveTag(Guid tagId)
        {
            Tag? tag = Tags.FirstOrDefault(t => t.Id == tagId);
            if (tag == null)
                throw new InvalidOperationException("Tag not found.");
            Tags.Remove(tag);
        }
        #endregion Methods
    }
}