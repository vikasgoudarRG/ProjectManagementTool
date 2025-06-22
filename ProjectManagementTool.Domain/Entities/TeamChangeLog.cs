using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Entities
{
    public class TeamChangeLog
    {
        #region Fields
        public Guid Id { get; init; }
        public Guid TeamId { get; init; }
        public Team Team { get; private set; } = null!;
        public Guid ProjectId { get; init; }

        public ChangeType ChangeType { get; init; }
        public TeamPropertyType PropertyChanged { get; private set; }
        public Guid ChangedByUserId { get; init; }
        public User ChangedByUser { get; private set; } = null!;

        public string? OldValue { get; init; }
        public string? NewValue { get; init; }
        public DateTime CreatedOn { get; init; }
        #endregion Fields

        #region Constructors
        private TeamChangeLog() { }

        public TeamChangeLog(Guid teamId, Guid changedByUserId, ChangeType changeType, TeamPropertyType propertyChanged, string? oldValue, string? newValue)
        {
            Id = Guid.NewGuid();
            TeamId = teamId;
            ChangedByUserId = changedByUserId;
            ChangeType = changeType;
            PropertyChanged = propertyChanged;
            OldValue = oldValue;
            NewValue = newValue;
            CreatedOn = DateTime.UtcNow;
        }
        #endregion Constructors
    }
}