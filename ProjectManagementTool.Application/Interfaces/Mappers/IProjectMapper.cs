using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Mappers
{
    public interface IProjectMapper
    {
        public ProjectDTO ToDTO(Project project);
    }
}