namespace ProjectManagementTool.Domain.Entities
{
    public class Tag
    {
        // Fields
        public Guid Id { get; init; }

        private string _name = null!;
        public string Name
        {
            get => _name;
            private set => _name = IsValidName(value) ? value : throw new Exception($"Tag Name - {value} is invalid");
        }

        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

        // Constructors 
        private Tag() { }

        public Tag(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        // Static Methods
        private static bool IsValidName(string name)
        {
            return string.IsNullOrWhiteSpace(name) ? false : true;
        }
    }
}