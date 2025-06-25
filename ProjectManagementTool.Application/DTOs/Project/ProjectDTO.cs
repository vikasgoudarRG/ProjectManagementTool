using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.Project
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid ProjectLeadId { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<UserDTO> Developers { get; set; } = null!;
    }
}