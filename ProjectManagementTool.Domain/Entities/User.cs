using ProjectManagementTool.Domain.Enums.User;

namespace ProjectManagementTool.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserRole Role { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();

    }
}