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
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        private Project() { }

        public Project(string title, string description, Guid managerId, ProjectStatus status, ICollection<User> developers)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception("Title cannot be null or white-spaced");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new Exception("The description cannot be null or white-spaced");
            }

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            ManagerId = managerId;
            Status = status;
            
        }
    }
}