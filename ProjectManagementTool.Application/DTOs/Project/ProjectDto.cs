using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.Project
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedOn { get; set; }

        public Guid ManagerId { get; set; }
        public string ManagerUsername { get; set; } = null!;
        public ICollection<SimpleUserDto> Developers { get; set; } = new List<SimpleUserDto>();
    }
}