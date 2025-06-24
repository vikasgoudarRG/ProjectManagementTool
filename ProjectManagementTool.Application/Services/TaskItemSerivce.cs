using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.Common;
using ProjectManagementTool.Domain.Enums.TaskItem;
using ProjectManagementTool.Domain.Exceptions;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChangeLogService _changeLogService;

        public TaskItemService(
            ITaskItemRepository taskRepository,
            IProjectRepository projectRepository,
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            IChangeLogService changeLogService,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _changeLogService = changeLogService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateTaskAsync(TaskItemCreateDTO dto, Guid creatorUserId)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId)
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != creatorUserId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, creatorUserId))
                throw new UnauthorizedAccessException("Only leads can create tasks.");

            var task = new TaskItem(dto.Title, dto.Description, team.Id, ParseType(dto.Type), ParsePriority(dto.Priority), ParseStatus(dto.Status), dto.Deadline);

            if (dto.AssignedUserId.HasValue)
            {
                var userId = dto.AssignedUserId.Value;
                if (!await _teamRepository.IsUserInTeamAsync(team.Id, userId))
                    throw new DomainException("Assigned user is not in team.");
                task.AssignTo(userId);
            }

            await _taskRepository.AddAsync(task);
            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog
            {
                TaskItemId = task.Id,
                ChangedByUserId = creatorUserId,
                PropertyChanged = "TaskItem",
                OldValue = null,
                NewValue = task.Title,
                ChangeType = ChangeType.Created
            });

            await _unitOfWork.SaveChangesAsync();
            return task.Id;
        }

        public async Task UpdateAsync(UpdateTaskItemDTO dto, Guid updaterUserId)
        {
            var task = await _taskRepository.GetByIdAsync(dto.Id)
                       ?? throw new NotFoundException("Task not found");

            var team = await _teamRepository.GetByIdAsync(task.TeamId)
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != updaterUserId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, updaterUserId))
                throw new UnauthorizedAccessException("Only leads can update.");

            string oldTitle = task.Title;

            task.Update(dto.Title, dto.Description, ParseStatus(dto.Status), ParsePriority(dto.Priority), ParseType(dto.Type), dto.Deadline, dto.AssignedUserId);

            await _taskRepository.UpdateAsync(task);
            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog
            {
                TaskItemId = task.Id,
                ChangedByUserId = updaterUserId,
                PropertyChanged = "Title",
                OldValue = oldTitle,
                NewValue = dto.Title,
                ChangeType = ChangeType.Updated
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid taskId, Guid requesterId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                       ?? throw new NotFoundException("Task not found");

            var team = await _teamRepository.GetByIdAsync(task.TeamId)
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, requesterId))
                throw new UnauthorizedAccessException("Only leads can delete tasks.");

            await _taskRepository.DeleteAsync(task);

            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog
            {
                TaskItemId = task.Id,
                ChangedByUserId = requesterId,
                PropertyChanged = "TaskItem",
                OldValue = task.Title,
                NewValue = null,
                ChangeType = ChangeType.Deleted
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssignAsync(AssignTaskItemDTO dto)
        {
            var task = await _taskRepository.GetByIdAsync(dto.TaskItemId)
                       ?? throw new NotFoundException("Task not found");

            var team = await _teamRepository.GetByIdAsync(task.TeamId)
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId &&
                !await _teamRepository.IsTeamLeadAsync(team.Id, dto.RequesterId))
                throw new UnauthorizedAccessException("Only leads can assign tasks.");

            if (!await _teamRepository.IsUserInTeamAsync(team.Id, dto.AssignedUserId))
                throw new DomainException("User not in team.");

            var oldAssignee = task.AssignedUserId?.ToString();
            task.AssignTo(dto.AssignedUserId);

            await _taskRepository.UpdateAsync(task);
            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog
            {
                TaskItemId = task.Id,
                ChangedByUserId = dto.RequesterId,
                PropertyChanged = "AssignedUserId",
                OldValue = oldAssignee,
                NewValue = dto.AssignedUserId.ToString(),
                ChangeType = ChangeType.Updated
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(ChangeTaskItemStatusDTO dto)
        {
            var task = await _taskRepository.GetByIdAsync(dto.TaskItemId)
                       ?? throw new NotFoundException("Task not found");

            if (task.AssignedUserId != dto.UserId)
                throw new UnauthorizedAccessException("Only assignee can change status.");

            var oldStatus = task.Status.ToString();
            var newStatus = dto.NewStatus;

            task.ChangeStatus(ParseStatus(newStatus));

            await _taskRepository.UpdateAsync(task);
            await _changeLogService.AddTaskItemLogAsync(new TaskItemChangeLog
            {
                TaskItemId = task.Id,
                ChangedByUserId = dto.UserId,
                PropertyChanged = "Status",
                OldValue = oldStatus,
                NewValue = newStatus,
                ChangeType = ChangeType.Updated
            });

            await _unitOfWork.SaveChangesAsync();
        }    

        private TaskItemDTO MapToDTO(TaskItem task)
        {
            return new TaskItemDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Type = task.Type.ToString(),
                Priority = task.Priority.ToString(),
                Status = task.Status.ToString(),
                AssignedUserId = task.AssignedUserId,
                TeamId = task.TeamId,
                CreatedAt = task.CreatedOn,
                Deadline = task.Deadline,
                CompletedAt = task.CompletedAt
            };
        }

        private TaskItemPriority ParsePriority(string str)
        {
            return Enum.TryParse<TaskItemPriority>(str, true, out var val)
                ? val
                : throw new InvalidEnumException("Invalid priority.");
        }

        private TaskItemStatus ParseStatus(string str)
        {
            return Enum.TryParse<TaskItemStatus>(str, true, out var val)
                ? val
                : throw new InvalidEnumException("Invalid status.");
        }

        private TaskItemType ParseType(string str)
        {
            return Enum.TryParse<TaskItemType>(str, true, out var val)
                ? val
                : throw new InvalidEnumException("Invalid type.");
        }
    }
}
