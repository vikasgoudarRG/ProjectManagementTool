using ProjectManagementTool.Application.DTOs.Project;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<Guid> CreateProjectAsync(CreateProjectRequestDto dto);
        Task UpdateProjectAsync(UpdateProjectRequestDto dto);
        Task DeleteProjectAsync(Guid projectId);
        Task AssignUsersToProjectAsync(AssignProjectDevelopers dto);
        Task<ProjectDto> GetProjectByIdAsync(Guid projectId);
        Task<ICollection<ProjectDto>> GetProjectsForUserAsync(Guid userId);
        Task<ICollection<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectSummaryDto> GetProjectSummaryAsync(Guid projectId);
    }
}