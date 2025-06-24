using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<Guid> CreateProjectAsync(CreateProjectDTO dto);
        Task<ProjectDTO?> GetByIdAsync(Guid projectId);
        Task<IEnumerable<ProjectDTO>> GetAllForUserAsync(Guid userId);
        Task DeleteProjectAsync(Guid projectId, Guid requesterId);

        Task AddDeveloperAsync(ProjectUserActionDTO dto);
        Task RemoveDeveloperAsync(ProjectUserActionDTO dto);
        Task<IEnumerable<UserDTO>> GetAllDevelopersAsync(Guid projectId, Guid requesterId);
    }

}