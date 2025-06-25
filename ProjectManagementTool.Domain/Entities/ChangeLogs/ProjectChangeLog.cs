using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Entities.ChangeLogs
{
    public class ProjectChangeLog : ChangeLogBase
    {
        #region Fields
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;
        #endregion Fields

        #region Constructors
        public ProjectChangeLog(
            Guid projectId,
            Guid changedByUserId,
            ChangeType changeType,
            string propertyChanged,
            string? oldValue,
            string? newValue)
        : base(
            changedByUserId,
            changeType,
            propertyChanged,
            oldValue,
            newValue)
        {
            ProjectId = projectId;
        }
        #endregion Constructors

        #region Methods
        public override string ToString()
        {
            return $"ProjectChangeLog | {PropertyChanged} | {ChangeType} | {OldValue} -> {NewValue} | By: {ChangedByUserId} at {CreatedOn} | TaskItemId: {ProjectId}";
        }
        #endregion Methods
    }
}