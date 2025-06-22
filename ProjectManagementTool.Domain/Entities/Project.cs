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
            set => _name = ValidateName(value);
        }

        private string _description = null!;
        public string Description
        {
            get => _description;
            set => _description = ValidateDescription(value);
        }

        public ProjectStatus Status { get; set; }
        public DateTime CreatedOn { get; init; }

        private readonly List<Team> _teams = new List<Team>();
        public IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();

        private readonly List<User> _developers = new List<User>();
        public IReadOnlyCollection<User> Developers => _developers.AsReadOnly();
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
        private static string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name), "Project name cannot be null or empty.");
            }
            return name;
        }

        private static string ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException(nameof(description), "Project description cannot be null or empty.");
            }
            return description;
        }

        public void AddTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team), "Team cannot be null.");
            if (team.ProjectId != Id)
            {
                throw new InvalidOperationException(nameof(team) + "Team is not associated with this project");
            }
            if (!_teams.Any(t => t.Id == team.Id))
            {
                _teams.Add(team);
            }
        }
        public void RemoveTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team), "Team cannot be null.");
            if (team.ProjectId != Id)
            {
                throw new InvalidOperationException(nameof(team) + "Team is not associated with this project");
            }
            if (_teams.Any(t => t.Id == team.Id))
            {
                _teams.Remove(team);
            }
        }

        public void AddDeveloper(User developer)
        {
            if (developer == null) throw new ArgumentNullException(nameof(developer), "Developer cannot be null.");
            if (!_developers.Any(d => d.Id == developer.Id))
            {
                _developers.Add(developer);
            }
        }
        public void RemoveDeveloper(User developer)
        {
            if (developer == null) throw new ArgumentNullException(nameof(developer), "Developer cannot be null");
            if (_developers.Any(d => d.Id == developer.Id))
            {
                _developers.Remove(developer);
            }
        }
        #endregion Methods
    }
}