namespace ProjectManagementTool.Domain.Entities
{
    public class User
    {
        #region Fields
        public Guid Id { get; init; }

        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = ValidateName(value);
        }

        private string _email = null!;
        public string Email
        {
            get => _email;
            set => _email = ValidateEmail(value);
        }
        #endregion Fields

            #region Constructors
        private User() { }

        public User(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }
        #endregion Constructors

        #region Methods
        public static string ValidateName(string name)
        {
            return string.IsNullOrWhiteSpace(name) ? throw new ArgumentException(nameof(name), "Name cannot be null or whitespace") : name;
        }

        private static string ValidateEmail(string email)
        {
            return string.IsNullOrWhiteSpace(email) ? throw new ArgumentException(nameof(email), "Email cannot be null or whitespace") : email;
        }
        #endregion Methods
    }
}