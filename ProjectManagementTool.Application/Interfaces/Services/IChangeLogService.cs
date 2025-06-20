using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IChangeLogService
    {
        Task CreateLogChangeAsync(TaskItemChangeLogDto dto);
        Task<ICollection<TaskItemChangeLogDto>> GetChangeLogsByTaskIdAsync(Guid taskId);
    }
}