using ProjectManagementTool.Application.DTOs.Project;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(Guid creatorUserId, CreateProjectDto dto);
        Task<ProjectDto> GetProjectByIdAsync(Guid projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjectsForUserAsync(Guid userId);
        Task<IEnumerable<ProjectDto>> GetAllProjectsByFilterAsync(FilterProjectDto filterProjectDto);
        Task<ProjectSummaryDto> GetProjectSummaryAsync(Guid projectId);
        Task AssignDevelopersAsync(Guid projectId, IEnumerable<Guid> devIds);
        Task UpdateProjectAsync(Guid projectId, UpdateProjectDto dto);
        Task MarkProjectStatusAsync(Guid projectId, string status);

        Task RemoveDevelopersAsync(Guid projectId, IEnumerable<Guid> userIds);
        Task DeleteProjectAsync(Guid projectId);        
    }
}