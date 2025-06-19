using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; init; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public Guid ManagerId { get; set; }
        public User Manager { get; set; } = null!;
        public ProjectStatus Status { get; private set; }

        public ICollection<User> Developers { get; set; } = new List<User>();
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

        private Project() { }

        public Project(string title, string description, Guid managerId)
        {
            setTitle(title);
            setDescription(description);
            setProjectStatus(ProjectStatus.Active.ToString());
        }

        public void setTitle(string title)
        {
            Title = (!string.IsNullOrWhiteSpace(title)) ? title : throw new Exception("Title invalid");
        }

        public void setDescription(string description)
        {
            Description = description;
        }

        public void setProjectStatus(string statusString)
        {
            Status = Enum.TryParse<ProjectStatus>(statusString, ignoreCase: true, out ProjectStatus status) ? status : throw new Exception("Status is invalid");
        }
    }
}