using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.Team;

namespace ProjectManagementTool.Domain.Entities
{
    public class Team
    {
        #region Fields
        public Guid Id { get; init; }
        public Guid ProjectId { get; init; }
        public Project Project { get; private set; } = null!; // navigation propertipropertyes
        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = ValidateAndGetName(value);
        }

        public ICollection<TeamMember> TeamMembers { get; private set; } = new List<TeamMember>();
        public ICollection<TaskItem> TaskItems { get; private set; } = new List<TaskItem>();

        public ICollection<TeamChangeLog> ChangeLogs { get; private set; } = new List<TeamChangeLog>();

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
        // =============== static methods ===============
        private static string ValidateAndGetName(string name)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            return name;
        }

        // =============== methods ===============
        public void AddMember(TeamMember member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            if (member.TeamId != Id)
                throw new InvalidOperationException("Member does not belong to this team");

            if (!TeamMembers.Any(m => m.UserId == member.UserId))
                TeamMembers.Add(member);
        }

        public void RemoveMember(TeamMember member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            if (member.TeamId != Id)
                throw new InvalidOperationException("Member does not belong to this team");

            TeamMember? existing = TeamMembers.FirstOrDefault(m => m.UserId == member.UserId);
            if (existing != null) TeamMembers.Remove(existing);
        }

        public void AssignTeamLead(Guid userId)
        {
            TeamMember? member = TeamMembers.FirstOrDefault(m => m.UserId == userId);
            if (member == null)
                throw new InvalidOperationException("User is not a member of this team");

            if (TeamMembers.Any(m => m.Role == TeamMemberRole.Lead))
                throw new InvalidOperationException("This team already has a lead");

            member.UpdateRole(TeamMemberRole.Lead);
        }

        public void RemoveTeamLead(Guid userId)
        {
            TeamMember? member = TeamMembers.FirstOrDefault(m => m.UserId == userId);
            if (member == null)
                throw new InvalidOperationException("User is not a member of this team");
            if (member.Role != TeamMemberRole.Lead)
                throw new InvalidOperationException("User is not the team lead");

            member.UpdateRole(TeamMemberRole.Developer);
        }
        #endregion Methods
    }
}