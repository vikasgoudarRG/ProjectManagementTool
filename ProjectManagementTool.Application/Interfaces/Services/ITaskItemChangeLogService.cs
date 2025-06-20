using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemChangeLogService
    {
        Task CreateLogChangeAsync(TaskItemChangeLogDto dto);
        Task<ICollection<TaskItemChangeLogDto>> GetChangeLogsByTaskIdAsync(Guid taskId);
    }
}