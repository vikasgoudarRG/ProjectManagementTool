namespace ProjectManagementTool.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = null!;
        public string Email { get; private set; } = null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Project> ManagedProjects { get; set; } = new List<Project>();

        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        private User() { }

        public User(string username, string email)
        {
            Id = Guid.NewGuid();
            SetEmail(email);
            SetUsername(username);
        }

        public void SetEmail(string email)
        {
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new Exception("Email is invalid");
        }

        public void SetUsername(string username)
        {
            Username = !string.IsNullOrWhiteSpace(username) ? username : throw new Exception("Username is invalid");
        }
    }
}