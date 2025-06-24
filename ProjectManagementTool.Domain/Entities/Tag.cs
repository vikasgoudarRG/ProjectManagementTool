namespace ProjectManagementTool.Domain.Entities
{
    public class Tag
    {
        #region Fields
        public Guid Id { get; init; }

        private string _name = null!;
        public string Name
        {
            get => _name;
            private set => _name = ValidateAndGetName(value);
        }
        private List<TaskItem> _tasks = new List<TaskItem>();
        public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();
        #endregion Fields

        #region Constructors
        private Tag() { }

        public Tag(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        #endregion Constructors

        #region Methods
        private static string ValidateAndGetName(string name)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or whitespace", nameof(name));
            return name;
        }
        #endregion Methods
    }
}