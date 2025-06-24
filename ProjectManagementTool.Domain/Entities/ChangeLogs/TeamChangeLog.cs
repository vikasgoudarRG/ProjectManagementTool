using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Entities.ChangeLogs
{
    public class TeamChangeLog : ChangeLogBase
    {
        #region Fields
        public Guid TeamId { get; init; }
        public Team Team { get; private set; } = null!;
        #endregion Fields

        #region Constructors
        public TeamChangeLog(Guid teamId, Guid changedByUserId, ChangeType changeType, string propertyChanged, string? oldValue, string? newValue)
        : base(changedByUserId, changeType, propertyChanged, oldValue, newValue)
        {
            TeamId = teamId;
        }
        #endregion Constructors

        #region Methods
        public override string ToString()
        {
            return $"TeamChangeLog | {PropertyChanged} | {ChangeType} | {OldValue} -> {NewValue} | By: {ChangedByUserId} at {CreatedOn} | TaskItemId: {TeamId}";
        }
        #endregion Methods
    }
}