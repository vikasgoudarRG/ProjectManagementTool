using ProjectManagementTool.Application.DTOs;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemChangeLogService
    {
        Task CreateLogChangeAsync(CreateTaskItemChangeLogDto dto);
        Task<TaskItemChangeLogDto> GetChangeLogByIdAsync(Guid id); 
        Task<IEnumerable<TaskItemChangeLogDto>> GetChangeLogsByTaskIdAsync(Guid taskId);
    }
}