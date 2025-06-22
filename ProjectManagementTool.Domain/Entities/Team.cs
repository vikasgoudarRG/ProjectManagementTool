namespace ProjectManagementTool.Domain.Entities
{
    public class Team
    {
        #region Fields
        public Guid Id { get; init; }
        public Guid ProjectId { get; init; }

        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = ValidateName(value);
        }

        // Navigation property to Project 
        public Project Project { get; private set; } = null!;

        // Navigation properties to Leads
        private readonly List<User> _leads = new();
        public IReadOnlyCollection<User> Leads => _leads.AsReadOnly();

        // Navigation properties to Developers
        private readonly List<User> _developers = new();
        public IReadOnlyCollection<User> Developers => _developers.AsReadOnly();

        // Navigation properties to TaskItems
        private readonly List<TaskItem> _taskItems = new();
        public IReadOnlyCollection<TaskItem> TaskItems => _taskItems.AsReadOnly();

        public DateTime CreatedOn { get; init; }
        #endregion Fields

        #region Constructors
        private Team() { }

        public Team(string name, Guid projectId)
        {
            Id = Guid.NewGuid();
            Name = name;
            ProjectId = projectId;
            CreatedOn = DateTime.UtcNow;
        }
        #endregion Constructors

        #region Methods
        private static string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name), "Team name cannot be null or empty");
            return name.Trim();
        }

        public void AddLead(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (!_leads.Any(u => u.Id == user.Id))
                _leads.Add(user);
        }

        public void RemoveLead(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _leads.RemoveAll(u => u.Id == user.Id);
        }

        public void AddDeveloper(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (!_developers.Any(u => u.Id == user.Id))
                _developers.Add(user);
        }

        public void RemoveDeveloper(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _developers.RemoveAll(u => u.Id == user.Id);
        }

        public void AddTaskItem(TaskItem taskItem)
        {
            if (taskItem == null) throw new ArgumentNullException(nameof(taskItem));
            if (!_taskItems.Any(t => t.Id == taskItem.Id))
                _taskItems.Add(taskItem);
        }

        public void RemoveTaskItem(TaskItem taskItem)
        {
            if (taskItem == null) throw new ArgumentNullException(nameof(taskItem));
            _taskItems.RemoveAll(t => t.Id == taskItem.Id);
        }
        #endregion Methods
    }
}