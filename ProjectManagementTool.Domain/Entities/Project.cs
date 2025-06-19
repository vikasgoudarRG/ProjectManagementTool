using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ManagerId { get; set; }
        public User Manager { get; set; } = null!;
        public ProjectStatus Status { get; set; }

        public ICollection<User> Developers { get; set; } = new List<User>();
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}