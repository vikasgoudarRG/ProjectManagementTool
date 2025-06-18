using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IChangeLogService
    {
        Task LogChangeAsync(TaskItemChangeLogDto dto);
        Task<ICollection<TaskItemChangeLogDto>> GetChangeLogsByTaskIdAsync(Guid taskId);
    }
}