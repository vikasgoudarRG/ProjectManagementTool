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
            private set => _name = IsValidName(value) ? value : throw new Exception($"Invalid Tag Name - {value}");
        }
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
        private static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
        #endregion Methods
    }
}