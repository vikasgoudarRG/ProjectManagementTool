using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Entities.ChangeLogs
{
    public class TaskItemChangeLog : ChangeLogBase
    {
        #region Fields
        public Guid TaskItemId { get; init; }
        public TaskItem TaskItem { get; private set; } = null!;
        #endregion Fields

        #region Constructors
        public TaskItemChangeLog(Guid taskItemId, Guid changedByUserId, ChangeType changeType, string propertyChanged, string? oldValue, string? newValue)
        : base(changedByUserId, changeType, propertyChanged, oldValue, newValue)
        {
            TaskItemId = taskItemId;
        }
        #endregion Constructors

        #region Methods
        public override string ToString()
        {
            return $"TaskItemChangeLog | {PropertyChanged} | {ChangeType} | {OldValue} -> {NewValue} | By: {ChangedByUserId} at {CreatedOn} | TaskItemId: {TaskItemId}";
        }
        #endregion Methods
    }
}