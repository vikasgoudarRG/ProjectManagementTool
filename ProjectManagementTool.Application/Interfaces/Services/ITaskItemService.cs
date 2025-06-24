using ProjectManagementTool.Application.DTOs.TaskItem;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemService
    {
        Task<Guid> CreateTaskAsync(TaskItemCreateDTO dto, Guid creatorUserId);
        Task<TaskItemDTO?> GetByIdAsync(Guid taskId, Guid requesterId);

        Task<IEnumerable<TaskItemDTO>> GetByProjectAsync(Guid projectId, Guid requesterId);
        Task<IEnumerable<TaskItemDTO>> GetByTeamAsync(Guid teamId, Guid requesterId);
        Task<IEnumerable<TaskItemDTO>> GetByUserAsync(Guid userId);

        Task UpdateAsync(UpdateTaskItemDTO dto, Guid updaterUserId);
        Task DeleteAsync(Guid taskId, Guid requesterId);

        Task AssignAsync(AssignTaskItemDTO dto);
        Task ChangeStatusAsync(ChangeTaskItemStatusDTO dto);
    }

}