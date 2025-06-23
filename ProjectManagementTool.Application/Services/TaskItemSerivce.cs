using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.TaskItem;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITaskItemChangeLogRepository _logRepo;
        private readonly IProjectRepository _projectRepo;

        public TaskItemService(
            ITaskItemRepository taskRepo,
            ITeamRepository teamRepo,
            IUserRepository userRepo,
            ITaskItemChangeLogRepository logRepo,
            IProjectRepository projectRepo)
        {
            _taskRepo = taskRepo;
            _teamRepo = teamRepo;
            _userRepo = userRepo;
            _logRepo = logRepo;
            _projectRepo = projectRepo;
        }

        public async Task<Guid> CreateTaskAsync(CreateTaskItemDTO dto, Guid creatorUserId)
        {
            var team = await _teamRepo.GetByIdAsync(dto.TeamId)
                       ?? throw new ArgumentException("Invalid team");
            var project = await _projectRepo.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");
            var isLead = project.ProjectLeadId == creatorUserId;
            var isTeamLead = await _teamRepo.IsUserInTeamAsync(dto.TeamId, creatorUserId);

            if (!isLead && !isTeamLead)
                throw new UnauthorizedAccessException();

            var task = new TaskItem(dto.Title, dto.Description, dto.TeamId);
            await _taskRepo.AddAsync(task);
            return task.Id;
        }

        public async Task UpdateTaskAsync(UpdateTaskItemDTO dto, Guid updaterUserId)
        {
            var task = await _taskRepo.GetByIdAsync(dto.TaskItemId)
                       ?? throw new ArgumentException("Task not found");
            var team = await _teamRepo.GetByIdAsync(task.TeamId)
                       ?? throw new ArgumentException("Team not found");
            var project = await _projectRepo.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");
            var isLead = project.ProjectLeadId == updaterUserId;
            var isTeamLead = await _teamRepo.IsUserInTeamAsync(task.TeamId, updaterUserId);

            if (!isLead && !isTeamLead)
                throw new UnauthorizedAccessException();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Deadline = dto.Deadline;

            await _taskRepo.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(Guid taskId, Guid requestingUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                       ?? throw new ArgumentException("Task not found");
            var team = await _teamRepo.GetByIdAsync(task.TeamId)
                       ?? throw new ArgumentException("Team not found");
            var project = await _projectRepo.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");
            var isLead = project.ProjectLeadId == requestingUserId;
            var isTeamLead = await _teamRepo.IsUserInTeamAsync(task.TeamId, requestingUserId);

            if (!isLead && !isTeamLead)
                throw new UnauthorizedAccessException();

            await _taskRepo.DeleteAsync(task);
        }

        public async Task<TaskItemDTO?> GetTaskByIdAsync(Guid taskId, Guid requestingUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) return null;

            var project = await _projectRepo.GetByIdAsync(task.Team.ProjectId)
                          ?? throw new ArgumentException("Project not found");
            var isLead = project.ProjectLeadId == requestingUserId;
            var isTeamLead = await _teamRepo.IsUserInTeamAsync(task.TeamId, requestingUserId);
            var isOwner = task.AssignedUserId == requestingUserId;

            if (!isLead && !isTeamLead && !isOwner)
                throw new UnauthorizedAccessException();

            return new TaskItemDTO(task.Id, task.Title, task.Status);
        }

        public async Task<IEnumerable<TaskItemDTO>> GetTasksByProjectAsync(Guid projectId, Guid requestingUserId)
        {
            var project = await _projectRepo.GetByIdAsync(projectId)
                          ?? throw new ArgumentException("Project not found");
            if (project.ProjectLeadId != requestingUserId)
                throw new UnauthorizedAccessException();

            var tasks = await _taskRepo.GetAllByProjectId(projectId);
            return tasks.Select(t => new TaskItemDTO(t.Id, t.Title, t.Status));
        }

        public async Task<IEnumerable<TaskItemDTO>> GetTasksByTeamAsync(Guid teamId, Guid requestingUserId)
        {
            var team = await _teamRepo.GetByIdAsync(teamId)
                       ?? throw new ArgumentException("Team not found");
            var project = await _projectRepo.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");
            var isLead = project.ProjectLeadId == requestingUserId;
            var isTeamLead = await _teamRepo.IsUserInTeamAsync(teamId, requestingUserId);

            if (!isLead && !isTeamLead)
                throw new UnauthorizedAccessException();

            var tasks = await _taskRepo.GetAllByTeamId(teamId);
            return tasks.Select(t => new TaskItemDTO(t.Id, t.Title, t.Status));
        }

        public async Task<IEnumerable<TaskItemDTO>> GetTasksByAssignedUserAsync(Guid userId)
        {
            var tasks = await _taskRepo.GetAllByAssignedUserId(userId);
            return tasks.Select(t => new TaskItemDTO(t.Id, t.Title, t.Status));
        }

        public async Task ChangeTaskStatusAsync(Guid taskId, TaskStatus newStatus, Guid changerUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                       ?? throw new ArgumentException("Task not found");
            if (task.AssignedUserId != changerUserId)
                throw new UnauthorizedAccessException("Only assigned user can change status");

            task.Status = newStatus;
            await _taskRepo.UpdateAsync(task);
        }

        public async Task AssignTaskAsync(Guid taskId, Guid assignedUserId, Guid requestingUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                       ?? throw new ArgumentException("Task not found");
            var team = await _teamRepo.GetByIdAsync(task.TeamId)
                       ?? throw new ArgumentException("Team not found");
            var project = await _projectRepo.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");

            var isLead = project.ProjectLeadId == requestingUserId;
            var isTeamLead = await _teamRepo.IsUserInTeamAsync(task.TeamId, requestingUserId);

            if (!isLead && !isTeamLead)
                throw new UnauthorizedAccessException();

            var user = await _userRepo.GetByIdAsync(assignedUserId)
                       ?? throw new ArgumentException("User not found");

            task.AssignToUser(user);
            await _taskRepo.UpdateAsync(task);
        }
    }
}
