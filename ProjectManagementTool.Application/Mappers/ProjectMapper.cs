using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public class ProjectMapper : IProjectMapper
    {
        private readonly IUserMapper _userMapper;
        public ProjectMapper(IUserMapper userMapper)
        {
            _userMapper = userMapper;
        }

        public ProjectDTO ToDTO(Project project)
        {
            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status.ToString(),
                ProjectLeadId = project.ProjectLeadId,
                CreatedOn = project.CreatedOn,
                Developers = project.Developers.Select(d => _userMapper.ToDTO(d))
            };
        }
    }
}