using ProjectManagementTool.Domain.Enums.Team;
namespace ProjectManagementTool.Domain.Entities
{
    public class TeamMember
    {
        #region Fields
        public Guid TeamId { get; init; }
        public Team Team { get; private set; } = null!;

        public Guid UserId { get; init; }
        public User User { get; private set; } = null!;

        public TeamMemberRole Role { get; private set; }
        public DateTime JoinedOn { get; init; }
        #endregion Fields

        #region Constructors
        private TeamMember() { }

        public TeamMember(Guid teamId, Guid userId, TeamMemberRole role)
        {
            TeamId = teamId;
            UserId = userId;
            Role = role;
            JoinedOn = DateTime.UtcNow;
        }
        #endregion Constructors

    }
}