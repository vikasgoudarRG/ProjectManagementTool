using System.ComponentModel.DataAnnotations;

namespace ProjectManagementTool.Domain.Entities
{
    public class Team
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid ProjectId { get; init; }
        public Project Project { get; private set; } = null!;

        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = IsValidName(value) ? value : throw new ArgumentException($"Invalid Team Name: {value}");
        }

        private readonly List<User> _leads = new List<User>();
        public IReadOnlyCollection<User> Leads => _leads.AsReadOnly();

        private readonly List<User> _developers = new List<User>();
        public IReadOnlyCollection<User> Developers => _developers.AsReadOnly();

        private readonly List<TaskItem> _taskItems = new List<TaskItem>();
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
        private static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public void AddLead(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

            if (!Leads.Contains(user))
            {
                Leads.Add(user);
            }
        }

        public void RemoveLead(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");
            if (Leads.Contains(user))
            {
                Leads.Remove(user);
            }
        }

        public void AddDeverloper(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");
            if (!Developers.Contains(user))
            {
                Developers.Add(user);
            }
        }
        public void RemoveDeveloper(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");
            if (Developers.Contains(user))
            {
                Developers.Remove(user);
            }
        }
        #endregion Methods
    }
}