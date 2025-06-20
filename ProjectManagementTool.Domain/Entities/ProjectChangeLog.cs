namespace ProjectManagementTool.Domain.Entities
{
    public class ProjectChangeLog
    {
        #region Fields
        public Guid Id { get; init; }

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;

        public Guid ChangedByUserId { get; private set; }
        public User ChangedByUser { get; private set; } = null!;

        public string ChangeType { get; set; } = null!;
        public string PropertyChanged { get; private set; } = null!;
        public string OldValue { get; private set; } = null!;
        public string NewValue { get; private set; } = null!;
        public DateTime CreatedOn { get; init; }
        #endregion Fields

        #region Constructors
        private ProjectChangeLog() { }

        public ProjectChangeLog(Guid projectId, Guid changedByUserId,string changeType, string propertyChanged, string oldValue, string newValue)
        {
            Id = Guid.NewGuid();
            ProjectId = projectId;
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