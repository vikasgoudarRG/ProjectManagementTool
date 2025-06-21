using System.ComponentModel.DataAnnotations;

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
            set => _name = IsValidName(value) ? value : throw new Exception($"User Name - {value} is invalid");
        }

        private string _email = null!;
        public string Email
        {
            get => _email;
            set => _email = IsValidEmail(value) ? value : throw new Exception($"User Email - {value} is invalid");
        }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Project> LeadOfProejects { get; set; } = new List<Project>();
        public ICollection<Team> LeadOfTeams { get; set; } = new List<Team>();
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<TaskItemChangeLog> TaskItemChangeLogs { get; set; } = new List<TaskItemChangeLog>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
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
        public static bool IsValidName(string name)
        {
            return string.IsNullOrWhiteSpace(name) ? false : true;
        }

        private static bool IsValidEmail(string email)
        {
            return string.IsNullOrWhiteSpace(email) ? false : true;
        }
        #endregion Methods
    }
}