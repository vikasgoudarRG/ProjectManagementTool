using ProjectManagementTool.Application.DTOs.Project;

namespace ProjectManagementTool.Application.DTOs.User
{
    public class GetUserProjectsDto
    {
        public ICollection<ProjectDto> Managed { get; set; } = new List<ProjectDto>();
        public ICollection<ProjectDto> Developer { get; set; } = new List<ProjectDto>();
    }
}