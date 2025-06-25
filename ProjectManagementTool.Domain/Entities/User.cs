using System.Text.RegularExpressions;

namespace ProjectManagementTool.Domain.Entities
{
    public class User
    {
        // ======================= Fields ======================= //
        public Guid Id { get; init; }

        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = ValidateAndGetName(value);
        }

        private string _email = null!;
        public string Email
        {
            get => _email;
            set => _email = ValidateAndGetEmail(value);
        }

        private string _password = null!;
        public string Password
        {
            get => _password;
            set => _password = ValidateAndGetPassword(value);
        }

        // ==================== Constructors ==================== //
        private User() { }

        public User(string name, string email, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = password;
        }

        // ======================= Methods ====================== //
        public static string ValidateAndGetName(string name)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or white space", nameof(name));

            return name;
        }

        private static string ValidateAndGetEmail(string email)
        {
            email = email.Trim();
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email cannot be null or whitespace", nameof(email));
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) throw new ArgumentException("Incorrect Email Format", nameof(email));
            return email;
        }

        private static string ValidateAndGetPassword(string password)
        {
            password = password.Trim();
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password cannot be null or white space", nameof(password));
            return password;
        }
    }
}