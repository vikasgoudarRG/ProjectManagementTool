namespace ProjectManagementTool.Domain.Entities
{
    public class Team
    {
        #region Fields
        public Guid Id { get; init; }
        public Guid ProjectId { get; init; }
        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = ValidateName(value);
        }

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
        private static string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name), "Team name cannot be null or empty");
            return name.Trim();
        }
        #endregion Methods
    }
}