using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.DTOs.Team;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemService
    {
        Task<TaskItemDTO> CreateTaskAsync(TaskItemCreateDTO dto);
        Task<TaskItemDTO?> GetByIdAsync(Guid id, Guid requesterId);
        Task<IEnumerable<TaskItemDTO>> GetByProjectAsync(Guid projectId, Guid requesterId);
        Task<IEnumerable<TaskItemDTO>> GetByTeamAsync(Guid teamId, Guid requesterId);
        Task<IEnumerable<TaskItemDTO>> GetByUserAsync(Guid userId);
        Task<TaskItemDTO> UpdateAsync(Guid id, UpdateTaskItemDTO dto, Guid updaterId);
        Task<TaskItemDTO> AssignAsync(AssignTaskItemDTO dto);
        Task<TaskItemDTO> ChangeStatusAsync(Guid id, ChangeTaskItemStatusDTO dto);
        Task DeleteAsync(Guid id, Guid requesterId);
    }
}