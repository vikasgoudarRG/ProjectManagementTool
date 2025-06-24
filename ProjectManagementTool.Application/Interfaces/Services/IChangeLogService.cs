using ProjectManagementTool.Application.DTOs.ChangeLog;
using ProjectManagementTool.Domain.Entities.ChangeLogs;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IChangeLogService
    {
        Task AddProjectLogAsync(ProjectChangeLog log);
        Task AddTeamLogAsync(TeamChangeLog log);
        Task AddTaskItemLogAsync(TaskItemChangeLog log);

        Task<IEnumerable<ProjectChangeLogDTO>> GetProjectLogsAsync(Guid projectId, Guid requesterId);
        Task<IEnumerable<TeamChangeLogDTO>> GetTeamLogsAsync(Guid teamId, Guid requesterId);
        Task<IEnumerable<TaskItemChangeLogDTO>> GetTaskLogsAsync(Guid taskId, Guid requesterId);
    }

}