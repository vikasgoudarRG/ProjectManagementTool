using ProjectManagementTool.Domain.Enums.Team;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    
    public interface IProjectManagementService
    {
        // Project
        Task<Guid> CreateProjectAsync(string name, string description, Guid projectLeadId);
        Task AddDeveloperToProjectAsync(Guid projectId, Guid developerId, Guid requestingUserId);
        Task<IEnumerable<ProjectDTO>> GetAllProjectsForUserAsync(Guid userId);
        Task<ProjectDTO?> GetProjectByIdAsync(Guid projectId);

        // Team
        Task<Guid> CreateTeamAsync(Guid projectId, string teamName, Guid requestingUserId);
        Task AssignTeamLeadAsync(Guid teamId, Guid userId, Guid requestingUserId);
        Task AddUserToTeamAsync(Guid teamId, Guid userId, TeamMemberRole role, Guid requestingUserId);
        Task<IEnumerable<TeamDTO>> GetAllTeamsForProjectAsync(Guid projectId, Guid requestingUserId);
        Task<TeamDTO?> GetTeamByIdAsync(Guid teamId, Guid requestingUserId);

        // Task Progress Retrieval
        Task<IEnumerable<TaskItemDTO>> GetAllTasksForProjectAsync(Guid projectId, Guid requestingUserId);
        Task<IEnumerable<TaskItemDTO>> GetAllTasksForTeamAsync(Guid teamId, Guid requestingUserId);
        Task<IEnumerable<TaskItemDTO>> GetAllTasksForUserAsync(Guid userId);

        // Logs
        Task<IEnumerable<ProjectChangeLogDTO>> GetProjectChangeLogsAsync(Guid projectId, Guid requestingUserId);
        Task<IEnumerable<TeamChangeLogDTO>> GetTeamChangeLogsAsync(Guid teamId, Guid requestingUserId);
        Task<IEnumerable<TaskItemChangeLogDTO>> GetTaskItemChangeLogsAsync(Guid taskItemId, Guid requestingUserId);
    }
}