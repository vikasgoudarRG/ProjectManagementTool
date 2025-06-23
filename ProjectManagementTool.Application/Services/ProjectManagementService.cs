using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Enums.Team;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ITaskItemChangeLogRepository _changeLogRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TaskItemService(
            ITaskItemRepository taskItemRepository,
            ITaskItemChangeLogRepository changeLogRepository,
            ITeamRepository teamRepository,
            IUserRepository userRepository)
        {
            _taskItemRepository = taskItemRepository;
            _changeLogRepository = changeLogRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<Guid> CreateTaskAsync(TaskItemCreateDTO dto, Guid creatorId)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId) ?? throw new ArgumentException("Invalid team");
            var task = new TaskItem(dto.Title, dto.Description, dto.TeamId, creatorId);
            await _taskItemRepository.AddAsync(task);
            return task.Id;
        }

        public async Task AssignUserAsync(Guid taskId, Guid userId, Guid requestingUserId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId) ?? throw new ArgumentException("Task not found");
            var team = await _teamRepository.GetByIdAsync(task.TeamId) ?? throw new ArgumentException("Team not found");
            var isTeamLead = await _teamRepository.IsUserInTeamAsync(task.TeamId, requestingUserId);
            if (!isTeamLead) throw new UnauthorizedAccessException();

            var user = await _userRepository.GetByIdAsync(userId) ?? throw new ArgumentException("User not found");
            task.AssignUser(userId);
            await _taskItemRepository.UpdateAsync(task);
        }

        public async Task UpdateStatusAsync(Guid taskId, TaskStatus status, Guid requestingUserId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId) ?? throw new ArgumentException("Task not found");
            if (task.AssignedUserId != requestingUserId) throw new UnauthorizedAccessException("Only assignee can update status");

            var oldStatus = task.Status;
            task.Status = status;
            await _taskItemRepository.UpdateAsync(task);

            await _changeLogRepository.AddAsync(new TaskItemChangeLog(taskId, requestingUserId, ChangeType.Update, TaskItemPropertyType.Status, oldStatus.ToString(), status.ToString()));
        }

        public async Task UpdateTitleAsync(Guid taskId, string newTitle, Guid requestingUserId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId) ?? throw new ArgumentException("Task not found");
            var oldValue = task.Title;
            task.Title = newTitle;
            await _taskItemRepository.UpdateAsync(task);

            await _changeLogRepository.AddAsync(new TaskItemChangeLog(taskId, requestingUserId, ChangeType.Update, TaskItemPropertyType.Title, oldValue, newTitle));
        }

        public async Task UpdateDescriptionAsync(Guid taskId, string newDescription, Guid requestingUserId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId) ?? throw new ArgumentException("Task not found");
            var oldValue = task.Description;
            task.Description = newDescription;
            await _taskItemRepository.UpdateAsync(task);

            await _changeLogRepository.AddAsync(new TaskItemChangeLog(task.Id, requestingUserId, ChangeType.Update, TaskItemPropertyType.Description, oldValue, newDescription));
        }

        public async Task DeleteTaskAsync(Guid taskId, Guid requestingUserId)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId) ?? throw new ArgumentException("Task not found");
            var team = await _teamRepository.GetByIdAsync(task.TeamId) ?? throw new ArgumentException("Team not found");
            var isTeamLead = await _teamRepository.IsUserInTeamAsync(task.TeamId, requestingUserId);
            if (!isTeamLead) throw new UnauthorizedAccessException();

            await _taskItemRepository.DeleteAsync(task);
        }
    }
}