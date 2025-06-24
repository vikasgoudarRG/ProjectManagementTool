using ProjectManagementTool.Application.DTOs.ChangeLog;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Exceptions;
using ProjectManagementTool.Domain.Interfaces.Repositories;

namespace ProjectManagementTool.Application.Services
{
    public class ChangeLogService : IChangeLogService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly ITaskItemRepository _taskRepo;
        private readonly IProjectChangeLogRepository _projectLogRepo;
        private readonly ITeamChangeLogRepository _teamLogRepo;
        private readonly ITaskItemChangeLogRepository _taskLogRepo;

        public ChangeLogService(
            IProjectRepository projectRepo,
            ITeamRepository teamRepo,
            ITaskItemRepository taskRepo,
            IProjectChangeLogRepository projectLogRepo,
            ITeamChangeLogRepository teamLogRepo,
            ITaskItemChangeLogRepository taskLogRepo)
        {
            _projectRepo = projectRepo;
            _teamRepo = teamRepo;
            _taskRepo = taskRepo;
            _projectLogRepo = projectLogRepo;
            _teamLogRepo = teamLogRepo;
            _taskLogRepo = taskLogRepo;
        }

        public async Task<IEnumerable<ProjectChangeLogDTO>> GetProjectLogsAsync(Guid projectId, Guid requesterId)
        {
            var project = await _projectRepo.GetByIdAsync(projectId)
                ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only the project lead can view project logs.");

            var logs = await _projectLogRepo.GetAllByProjectIdAsync(projectId);
            return logs.Select(MapProjectLogToDTO);
        }

        public async Task<IEnumerable<TeamChangeLogDTO>> GetTeamLogsAsync(Guid teamId, Guid requesterId)
        {
            var team = await _teamRepo.GetByIdAsync(teamId)
                ?? throw new NotFoundException("Team not found");

            var isTeamLead = await _teamRepo.IsTeamLeadAsync(teamId, requesterId);
            var isProjectLead = (await _projectRepo.GetByIdAsync(team.ProjectId))?.ProjectLeadId == requesterId;

            if (!isTeamLead && !isProjectLead)
                throw new UnauthorizedAccessException("Only team leads or the project lead can view team logs.");

            var logs = await _teamLogRepo.GetAllByTeamIdAsync(teamId);
            return logs.Select(MapTeamLogToDTO);
        }

        public async Task<IEnumerable<TaskItemChangeLogDTO>> GetTaskLogsAsync(Guid taskId, Guid requesterId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                ?? throw new NotFoundException("Task not found");

            var team = await _teamRepo.GetByIdAsync(task.TeamId)
                ?? throw new NotFoundException("Team not found");

            var isTeamLead = await _teamRepo.IsTeamLeadAsync(task.TeamId, requesterId);
            var isProjectLead = (await _projectRepo.GetByIdAsync(team.ProjectId))?.ProjectLeadId == requesterId;
            var isAssignee = task.AssignedUserId == requesterId;

            if (!isTeamLead && !isProjectLead && !isAssignee)
                throw new UnauthorizedAccessException("You are not authorized to view this task's logs.");

            var logs = await _taskLogRepo.GetAllByTaskItemIdAsync(taskId);
            return logs.Select(MapTaskLogToDTO);
        }

        public async Task AddProjectLogAsync(ProjectChangeLog log)
        {
            await _projectLogRepository.AddAsync(log);
        }

        public async Task AddTeamLogAsync(TeamChangeLog log)
        {
            await _teamLogRepository.AddAsync(log);
        }

        public async Task AddTaskItemLogAsync(TaskItemChangeLog log)
        {
            await _taskItemLogRepository.AddAsync(log);
        }

        // Mapping Helpers
        private ProjectChangeLogDTO MapProjectLogToDTO(ProjectChangeLog log) => new()
        {
            Id = log.Id,
            ProjectId = log.ProjectId,
            ChangedByUserId = log.ChangedByUserId,
            PropertyChanged = log.PropertyChanged,
            OldValue = log.OldValue,
            NewValue = log.NewValue,
            ChangeType = log.ChangeType.ToString(),
            CreatedOn = log.CreatedOn
        };

        private TeamChangeLogDTO MapTeamLogToDTO(TeamChangeLog log) => new()
        {
            Id = log.Id,
            TeamId = log.TeamId,
            ChangedByUserId = log.ChangedByUserId,
            PropertyChanged = log.PropertyChanged,
            OldValue = log.OldValue,
            NewValue = log.NewValue,
            ChangeType = log.ChangeType.ToString(),
            CreatedOn = log.CreatedOn
        };

        private TaskItemChangeLogDTO MapTaskLogToDTO(TaskItemChangeLog log) => new()
        {
            Id = log.Id,
            TaskItemId = log.TaskItemId,
            ChangedByUserId = log.ChangedByUserId,
            PropertyChanged = log.PropertyChanged,
            OldValue = log.OldValue,
            NewValue = log.NewValue,
            ChangeType = log.ChangeType.ToString(),
            CreatedOn = log.CreatedOn
        };
    }
}
