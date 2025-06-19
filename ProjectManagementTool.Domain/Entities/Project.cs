using System.Net.Mail;
using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; init; }

        private string _title = null!;
        public string Title
        {
            get => _title;
            set => _title = IsValidTitle(value) ? value : throw new Exception($"Project Title - {value} is invalid");
        }

        private string _description = null!;
        public string Description
        {
            get => _description;
            set => _description = IsValidDescription(value) ? value : throw new Exception($"Project Description - {value} is invalid");
        }
        public Guid ManagerId { get; set; }
        public User Manager { get; set; } = null!;
        public ProjectStatus Status { get; set; }

        public ICollection<User> Developers { get; set; } = new List<User>();
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

        private Project() { }

        public Project(string title, string description, Guid managerId, ProjectStatus status, IEnumerable<User>? developers = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            ManagerId = managerId;
            Status = status;
            if (developers != null)
            {
                Developers = developers.ToList<User>();
            }
        }

        private static bool IsValidTitle(string title)
        {
            return string.IsNullOrWhiteSpace(title) ? false : true;
        }

        private static bool IsValidDescription(string description)
        {
            return string.IsNullOrWhiteSpace(description) ? false : true;
        }

        public void AddDeveloper(User developer)
        {
            if (!Developers.Any(d => d.Id == developer.Id))
            {
                Developers.Add(developer);
            }
        }

        public void AddDevelopers(IEnumerable<User> developers)
        {
            foreach (User developer in developers)
            {
                AddDeveloper(developer);
            }
        }
    }
}