using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Enums.TaskItem;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        // ======================= Fields ======================= //
        private readonly ITaskItemRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChangeLogService _changeLogService;
        private readonly ITaskItemMapper _taskItemMapper;
        private readonly IUserNotificationService _notificationService;

        // ==================== Constructors ==================== //
        public TaskItemService(
            ITaskItemRepository taskRepository,
            IProjectRepository projectRepository,
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            IChangeLogService changeLogService,
            IUserNotificationService notificationService,
            ITaskItemMapper taskItemMapper,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _changeLogService = changeLogService;
            _notificationService = notificationService;
            _taskItemMapper = taskItemMapper;
            _unitOfWork = unitOfWork;
        }

        // ======================= Methods ====================== //
        // Create
        public async Task<TaskItemDTO> CreateTaskAsync(TaskItemCreateDTO dto)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId)
                ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != dto.RequestorId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, dto.RequestorId))
                throw new UnauthorizedAccessException("Only leads can create tasks.");

            if (dto.AssignedUserId.HasValue &&
                !await _teamRepository.IsUserInTeamAsync(team.Id, dto.AssignedUserId.Value))
                throw new ArgumentException("Assigned user must be in the team.");

            var task = new TaskItem(
                dto.Title,
                dto.Description,
                ParseType(dto.Type),
                ParsePriority(dto.Priority),
                ParseStatus(dto.Status),
                dto.AssignedUserId,
                dto.Deadline)
            {
                TeamId = team.Id
            };

            await _taskRepository.AddAsync(task);

            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(
                task.Id, dto.RequestorId, ChangeType.Created, "TaskItem", null, dto.Title));

            if (dto.AssignedUserId.HasValue)
            {
                UserNotification userNotification = new UserNotification(
                    userId: dto.AssignedUserId.Value,
                    message: $"You have been assigned a new task: {dto.Title}"
                );
                await _notificationService.SendUserNotification(userNotification);
            }

            await _unitOfWork.SaveChangesAsync();
            return _taskItemMapper.ToDTO(task);
        }

        // Read
        public async Task<TaskItemDTO?> GetByIdAsync(Guid taskId, Guid requesterId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId) ?? throw new KeyNotFoundException("Task not found");

            var team = await _teamRepository.GetByIdAsync(task.TeamId)
                ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != requesterId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, requesterId) &&
                task.AssignedUserId != requesterId)
                throw new UnauthorizedAccessException("You are not authorized to view this task.");

            return _taskItemMapper.ToDTO(task);
        }

        public async Task<IEnumerable<TaskItemDTO>> GetByProjectAsync(Guid projectId, Guid requesterId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only the project lead can view all tasks.");

            var tasks = await _taskRepository.GetAllByProjectId(projectId);
            return tasks.Select(t => _taskItemMapper.ToDTO(t));
        }

        public async Task<IEnumerable<TaskItemDTO>> GetByTeamAsync(Guid teamId, Guid requesterId)
        {
            if (!await _teamRepository.IsUserInTeamAsync(teamId, requesterId) &&
                !await _teamRepository.IsTeamLeadAsync(teamId, requesterId))
                throw new UnauthorizedAccessException("Not authorized.");

            var tasks = await _taskRepository.GetAllByTeamId(teamId);
            return tasks.Select(t => _taskItemMapper.ToDTO(t));
        }

        public async Task<IEnumerable<TaskItemDTO>> GetByUserAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetAllByAssignedUserId(userId);
            return tasks.Select(t => _taskItemMapper.ToDTO(t));
        }

        // Update
        public async Task<TaskItemDTO> UpdateAsync(Guid id, UpdateTaskItemDTO dto, Guid updaterUserId)
        {
            var task = await _taskRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("Task not found");

            var team = await _teamRepository.GetByIdAsync(task.TeamId)
                ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != updaterUserId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, updaterUserId))
                throw new UnauthorizedAccessException("Only leads can update tasks.");

            if (dto.AssignedUserId.HasValue &&
                !await _teamRepository.IsUserInTeamAsync(team.Id, dto.AssignedUserId.Value))
                throw new ArgumentException("Assigned user must be in the team.");

            if (dto.Title != null && dto.Title != task.Title)
            {
                await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(task.Id, updaterUserId, ChangeType.Updated, "Title", task.Title, dto.Title));
                task.Title = dto.Title;
            }

            if (dto.Description != null && dto.Description != task.Description)
            {
                await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(task.Id, updaterUserId, ChangeType.Updated, "Description", task.Description, dto.Description));
                task.Description = dto.Description;
            }

            if (dto.Status != null)
            {
                var parsed = ParseStatus(dto.Status);
                if (parsed != task.Status)
                {
                    await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(task.Id, updaterUserId, ChangeType.Updated, "Status", task.Status.ToString(), dto.Status));
                    task.Status = parsed;
                }
            }

            if (dto.Priority != null)
            {
                var parsed = ParsePriority(dto.Priority);
                if (parsed != task.Priority)
                {
                    await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(task.Id, updaterUserId, ChangeType.Updated, "Priority", task.Priority.ToString(), dto.Priority));
                    task.Priority = parsed;
                }
            }

            if (dto.Type != null)
            {
                var parsed = ParseType(dto.Type);
                if (parsed != task.Type)
                {
                    await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(task.Id, updaterUserId, ChangeType.Updated, "Type", task.Type.ToString(), dto.Type));
                    task.Type = parsed;
                }
            }

            if (dto.Deadline != null && dto.Deadline != task.Deadline)
            {
                await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(task.Id, updaterUserId, ChangeType.Updated, "Deadline", task.Deadline?.ToString(), dto.Deadline?.ToString()));
                task.Deadline = dto.Deadline;
            }

            if (dto.AssignedUserId != null && dto.AssignedUserId != task.AssignedUserId)
            {
                var oldAssignee = task.AssignedUserId;

                await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(
                    task.Id, updaterUserId, ChangeType.Updated, "AssignedUserId", oldAssignee?.ToString(), dto.AssignedUserId.ToString()));

                task.AssignedUserId = dto.AssignedUserId;

                if (oldAssignee.HasValue)
                {
                    if (dto.AssignedUserId.HasValue)
                    {
                        UserNotification userNotification = new UserNotification(
                            userId: dto.AssignedUserId.Value,
                            message: $"You have been unassigned a new task: {dto.Title}"
                        );
                        await _notificationService.SendUserNotification(userNotification);
                    }
                }

                {
                    UserNotification userNotification = new UserNotification(
                        userId: dto.AssignedUserId.Value,
                        message: $"You have been assigned a new task: {dto.Title}"
                    );
                    await _notificationService.SendUserNotification(userNotification);
                }
            }

            else if (task.AssignedUserId != null)
            {
                UserNotification userNotification = new UserNotification(
                    userId: task.AssignedUserId.Value,
                    message: $"Task details have been updated: {task.Title}"
                );
                await _notificationService.SendUserNotification(userNotification);
            }

            await _taskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return _taskItemMapper.ToDTO(task);
        }

        public async Task<TaskItemDTO> AssignAsync(AssignTaskItemDTO dto)
        {
            var task = await _taskRepository.GetByIdAsync(dto.TaskItemId)
                ?? throw new ArgumentException("Task not found");

            var team = await _teamRepository.GetByIdAsync(task.TeamId)
                ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, dto.RequesterId))
                throw new UnauthorizedAccessException("Only leads can assign tasks.");

            if (!await _teamRepository.IsUserInTeamAsync(team.Id, dto.AssignedUserId))
                throw new ArgumentException("Assigned user is not in the team.");

            if (task.AssignedUserId == dto.AssignedUserId) return _taskItemMapper.ToDTO(task);

            var oldAssignee = task.AssignedUserId;
            task.AssignedUserId = dto.AssignedUserId;

            await _taskRepository.UpdateAsync(task);

            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(
                task.Id, dto.RequesterId, ChangeType.Updated, "AssignedUserId", oldAssignee?.ToString(), dto.AssignedUserId.ToString()));


            UserNotification userNotification = new UserNotification(
                userId: dto.AssignedUserId,
                message: $"You have been assigned to task: {task.Title}"
            );
            await _notificationService.SendUserNotification(userNotification);

            if (oldAssignee.HasValue && oldAssignee != dto.AssignedUserId)
            {
                userNotification = new UserNotification(
                    userId: dto.AssignedUserId,
                    message: $"You have been unassigned from task: {task.Title}"
                );
                await _notificationService.SendUserNotification(userNotification);
            }

            await _unitOfWork.SaveChangesAsync();
            return _taskItemMapper.ToDTO(task);
        }

        public async Task<TaskItemDTO> ChangeStatusAsync(Guid id, ChangeTaskItemStatusDTO dto)
        {
            var task = await _taskRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("Task not found");

            if (task.AssignedUserId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only the assignee can change the task status.");

            var newStatus = ParseStatus(dto.NewStatus);
            var oldStatus = task.Status;

            task.Status = newStatus;

            await _taskRepository.UpdateAsync(task);

            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(
                task.Id, dto.RequesterId, ChangeType.Updated, "Status", oldStatus.ToString(), dto.NewStatus));

            await _unitOfWork.SaveChangesAsync();
            return _taskItemMapper.ToDTO(task);
        }

        // Delete
        public async Task DeleteAsync(Guid taskId, Guid requesterId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new ArgumentException("Task not found");

            var team = await _teamRepository.GetByIdAsync(task.TeamId)
                ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != requesterId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, requesterId))
                throw new UnauthorizedAccessException("Only leads can delete tasks.");

            await _taskRepository.DeleteAsync(task);

            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog(
                task.Id, requesterId, ChangeType.Deleted, "TaskItem", task.Title, null));

            await _unitOfWork.SaveChangesAsync();
        }

        private TaskItemPriority ParsePriority(string str) =>
            Enum.TryParse(str, true, out TaskItemPriority result)
                ? result : throw new ArgumentException("Invalid priority");

        private TaskItemStatus ParseStatus(string str) =>
            Enum.TryParse(str, true, out TaskItemStatus result)
                ? result : throw new ArgumentException("Invalid status");

        private TaskItemType ParseType(string str) =>
            Enum.TryParse(str, true, out TaskItemType result)
                ? result : throw new ArgumentException("Invalid type");
    }
}
