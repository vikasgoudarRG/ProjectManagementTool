using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.Project
{
    public class ProjectSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public Guid ManagerId { get; set; }
        public string ManagerUsername { get; set; } = null!;

        public int TotalTaskItems { get; set; }
        public IDictionary<string, int> TaskItemCountByType { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, int> TaskItemCountByStatus { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, int> TaskItemCountByPriority { get; set; } = new Dictionary<string, int>();


        public ICollection<SimpleUserDto> Developers { get; set; } = new List<SimpleUserDto>();

    }
}