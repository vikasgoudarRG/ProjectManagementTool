using System.ComponentModel.DataAnnotations;

namespace ProjectManagementTool.Domain.Entities
{
    public class User
    {
        #region Fields
        public Guid Id { get; init; }

        private string _username = null!;
        public string Username
        {
            get => _username;
            set => _username = IsValidUsername(value) ? value : throw new Exception($"User Username - {value} is invalid");
        }

        private string _email = null!;
        public string Email
        {
            get => _email;
            set => _email = IsValidEmail(value) ? value : throw new Exception($"User Email - {value} is invalid");
        }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Project> ManagedProjects { get; set; } = new List<Project>();
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<TaskItemChangeLog> TaskItemChangeLogs { get; set; } = new List<TaskItemChangeLog>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        #endregion Fields

        #region Constructors
        private User() { }

        public User(string username, string email)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
        }
        #endregion Constructors

        #region Static Methods
        public static bool IsValidUsername(string username)
        {
            return string.IsNullOrWhiteSpace(username) ? false : true;
        }

        private static bool IsValidEmail(string email)
        {
            return string.IsNullOrWhiteSpace(email) ? false : true;
        }
        #endregion Constructors
    }
}