using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IProjectService
    {
        // Create
        Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO dto);
        // Read
        Task<ProjectDTO> GetByIdAsync(Guid projectId);
        Task<IEnumerable<ProjectDTO>> GetAllForUserAsync(Guid userId);
        Task<IEnumerable<UserDTO>> GetAllDevelopersAsync(Guid projectId, Guid requesterId);
        // Update
        Task AddDeveloperAsync(Guid requestorId, ProjectDeveloperDTO dto);
        Task RemoveDeveloperAsync(Guid requestorId, ProjectDeveloperDTO dto);
        Task UpdateAsync(Guid requestorId, Guid projectId, UpdateProjectDTO dto);
        // Remove
        Task DeleteProjectAsync(Guid projectId, Guid requesterId);


    }

}