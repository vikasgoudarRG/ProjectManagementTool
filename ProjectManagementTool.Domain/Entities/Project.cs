using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Domain.Entities
{
    public class Project
    {
        #region Fields
        public Guid Id { get; init; }

        private string _title = null!;
        public string Title
        {
            get => _title;
            private set => _title = IsValidTitle(value) ? value : throw new Exception($"Invalid Project Title: {value}");
        }

        private string _description = null!;
        public string Description
        {
            get => _description;
            private set => _description = IsValidDescription(value) ? value : throw new Exception($"Invalid Project Description: {value}");
        }

        public Guid ManagerId { get; set; }
        public User Manager { get; set; } = null!;

        public ProjectStatus Status { get; set; }
        public DateTime CreatedOn { get; init; }
        public ICollection<User> Developers { get; set; } = new List<User>();
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
        #endregion Fields

        #region Constructors
        private Project() { }

        public Project(string title, string description, Guid managerId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            ManagerId = managerId;
            Status = ProjectStatus.Active;
            CreatedOn = DateTime.UtcNow;
        }
        #endregion Constructors

        #region Methods
        private static bool IsValidTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        private static bool IsValidDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description);
        }
        #endregion Methods
    }
}