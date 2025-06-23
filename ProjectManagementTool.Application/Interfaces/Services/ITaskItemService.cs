namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemService
    {
        namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITaskItemService
    {
        Task<Guid> CreateTaskAsync(TaskItemCreateDTO dto, Guid creatorId);
        Task AssignUserAsync(Guid taskId, Guid userId, Guid requestingUserId);
        Task UpdateStatusAsync(Guid taskId, TaskStatus status, Guid requestingUserId);
        Task UpdateTitleAsync(Guid taskId, string newTitle, Guid requestingUserId);
        Task UpdateDescriptionAsync(Guid taskId, string newDescription, Guid requestingUserId);
        Task DeleteTaskAsync(Guid taskId, Guid requestingUserId);
    }
}

    }
}