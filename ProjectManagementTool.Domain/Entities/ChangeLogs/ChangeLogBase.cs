using ProjectManagementTool.Domain.Enums.ChangeLog;

namespace ProjectManagementTool.Domain.Entities.ChangeLogs
{
    public abstract class ChangeLogBase
    {
        #region Fields
        public Guid Id { get; init; }
        public Guid ChangedByUserId { get; init; }
        public User ChangedByUser { get; private set; } = null!;
        public ChangeType ChangeType { get; init; }
        public string PropertyChanged { get; init; } = null!;
        public string? OldValue { get; init; }
        public string? NewValue { get; init; }
        public DateTime CreatedOn { get; init; }
        #endregion Fields

        #region Constuctors
        protected ChangeLogBase() { }

        protected ChangeLogBase(
            Guid changedByUserId,
            ChangeType changeType,
            string propertyChanged,
            string? oldValue, string?
            newValue)
        {
            Id = Guid.NewGuid();
            ChangedByUserId = changedByUserId;
            ChangeType = changeType;
            PropertyChanged = propertyChanged;
            OldValue = oldValue;
            NewValue = newValue;
            CreatedOn = DateTime.UtcNow;
        }
        #endregion Constructors

        #region Methods
        public abstract override string ToString();
        #endregion Methods
    }   
}