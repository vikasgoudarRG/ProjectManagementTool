using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Application.QueryModels
{
    public class ProjectFilterQueryModel
    {
        public string? TitleKeyword { get; set; }
        public Guid? ManagerId { get; set; }
        public ProjectStatus? Status { get; set; }
        public IEnumerable<Guid>? DeveloperIds { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public DateTime? CreatedAfter { get; set; }
    }
}