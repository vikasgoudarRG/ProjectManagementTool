namespace ProjectManagementTool.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid ManagerId { get; set; }
        public User Manager { get; set; } = null!;
        public bool IsArchived { get; set; }

        public ICollection<User> Developers { get; set; } = new List<User>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}