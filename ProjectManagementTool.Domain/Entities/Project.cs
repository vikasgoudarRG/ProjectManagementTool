using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Domain.Entities
{
    public class Project
    {
        #region Fields
        public Guid Id { get; init; }
        public Guid ProjectLeadId { get; init; }
        public User ProjectLead { get; private set; } = null!; // Navigation property 
        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = ValidateAndGetName(value);
        }

        private string _description = null!;
        public string Description
        {
            get => _description;
            set => _description = ValidateAndGetDescription(value);
        }

        public ProjectStatus Status { get; set; }
        public DateTime CreatedOn { get; init; }

        public ICollection<Team> Teams { get; private set; } = new List<Team>();
        public ICollection<User> Developers { get; private set; } = new List<User>();
        public ICollection<ProjectChangeLog> ChangeLogs { get; private set; } = new List<ProjectChangeLog>();
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
        // =============== static methods ===============
        private static string ValidateAndGetName(string name)
        {
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Project name cannot be null or empty", nameof(name));
            }
            return name;
        }

        private static string ValidateAndGetDescription(string description)
        {
            description = description.Trim();
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Project description cannot be null or empty", nameof(description));
            }
            return description;
        }

        // =============== methods ===============
        public void AddTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            if (team.ProjectId != Id)
            {
                throw new InvalidOperationException("Team is not associated with this project");
            }
            if (!Teams.Any(t => t.Id == team.Id))
            {
                Teams.Add(team);
            }
        }
        public void RemoveTeam(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            if (team.ProjectId != Id)
            {
                throw new InvalidOperationException("Team is not associated with this project");
            }
            if (Teams.Any(t => t.Id == team.Id))
            {
                Teams.Remove(team);
            }
        }

        public void AddDeveloper(User developer)
        {
            if (developer == null) throw new ArgumentNullException(nameof(developer));
            if (!Developers.Any(d => d.Id == developer.Id))
            {
                Developers.Add(developer);
            }
        }
        public void RemoveDeveloper(User developer)
        {
            if (developer == null) throw new ArgumentNullException(nameof(developer));
            if (Developers.Any(d => d.Id == developer.Id))
            {
                Developers.Remove(developer);
            }
        }
        public bool IsMember(Guid userId)
        {
            return ProjectLeadId == userId || Developers.Any(d => d.Id == userId);
        }
        #endregion Methods
    }
}