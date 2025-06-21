using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Domain.Entities
{
    public class Project
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid ProjectLeadId { get; init; }
        public User ProjectLead { get; private set; } = null!;

        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = IsValidName(value) ? value : throw new Exception($"Invalid Project Name: {value}");
        }

        private string _description = null!;
        public string Description
        {
            get => _description;
            set => _description = IsValidDescription(value) ? value : throw new Exception($"Invalid Project Description: {value}");
        }

        public ProjectStatus Status { get; set; }
        public DateTime CreatedOn { get; init; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<User> Developers { get; set; } = new List<User>();
        #endregion Fields

        #region Constructors
        private Project() { }

        public Project(string name, string description, Guid projectLeadId)
        {
            Id = Guid.NewGuid();
            ProjectLeadId = projectLeadId;
            Name = name;
            Description = description;
            Status = ProjectStatus.Active;
            CreatedOn = DateTime.UtcNow;
        }
        #endregion Constructors

        #region Methods
        private static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        private static bool IsValidDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description);
        }
        #endregion Methods
    }
}