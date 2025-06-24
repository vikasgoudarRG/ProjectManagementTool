using ProjectManagementTool.Application.DTOs.ChangeLog;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Interfaces.Repositories;

namespace ProjectManagementTool.Application.Services
{
    public class ChangeLogService : IChangeLogService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly ITeamRepository _teamRepo;
        private readonly ITaskItemRepository _taskRepo;
        private readonly IProjectChangeLogRepository _projectLogRepository;
        private readonly ITeamChangeLogRepository _teamLogRepository;
        private readonly ITaskItemChangeLogRepository _taskItemLogRepository;

        public ChangeLogService(
            IProjectRepository projectRepo,
            ITeamRepository teamRepo,
            ITaskItemRepository taskRepo,
            IProjectChangeLogRepository projectLogRepo,
            ITeamChangeLogRepository teamLogRepo,
            ITaskItemChangeLogRepository taskItemLogRepo)
        {
            _projectRepo = projectRepo;
            _teamRepo = teamRepo;
            _taskRepo = taskRepo;
            _projectLogRepository = projectLogRepo;
            _teamLogRepository = teamLogRepo;
            _taskItemLogRepository = taskItemLogRepo;
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

        public async Task<IEnumerable<ProjectChangeLogDTO>> GetProjectLogsAsync(Guid projectId, Guid requesterId)
        {
            Project? project = await _projectRepo.GetByIdAsync(projectId)
                ?? throw new ArgumentException("Project not foud");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only the project lead can view project logs.");

            IEnumerable<ProjectChangeLog> logs = await _projectLogRepository.GetAllByProjectIdAsync(projectId);
            return logs.Select(MapProjectLogToDTO);
        }

        public async Task<IEnumerable<TeamChangeLogDTO>> GetTeamLogsAsync(Guid teamId, Guid requesterId)
        {
            Team? team = await _teamRepo.GetByIdAsync(teamId)
                ?? throw new ArgumentException("Team not found");

            bool isTeamLead = await _teamRepo.IsTeamLeadAsync(teamId, requesterId);
            Project? project = await _projectRepo.GetByIdAsync(team.ProjectId);
            bool isProjectLead = project != null && project.ProjectLeadId == requesterId;

            if (!isTeamLead && !isProjectLead)
                throw new UnauthorizedAccessException("Only team leads or the project lead can view team logs.");

            IEnumerable<TeamChangeLog> logs = await _teamLogRepository.GetAllByTeamIdAsync(teamId);
            return logs.Select(MapTeamLogToDTO);
        }

        public async Task<IEnumerable<TaskItemChangeLogDTO>> GetTaskLogsAsync(Guid taskId, Guid requesterId)
        {
            TaskItem task = await _taskRepo.GetByIdAsync(taskId)
                ?? throw new ArgumentException("Task not found");

            Team team = await _teamRepo.GetByIdAsync(task.TeamId)
                ?? throw new ArgumentException("Team not found");

            bool isTeamLead = await _teamRepo.IsTeamLeadAsync(task.TeamId, requesterId);
            bool isProjectLead = (await _projectRepo.GetByIdAsync(team.ProjectId))?.ProjectLeadId == requesterId;
            bool isAssignee = task.AssignedUserId == requesterId;

            if (!isTeamLead && !isProjectLead && !isAssignee)
                throw new UnauthorizedAccessException("You are not authorized to view this task's logs.");

            var logs = await _taskItemLogRepository.GetAllByTaskItemIdAsync(taskId);
            return logs.Select(MapTaskLogToDTO);
        }
        private ProjectChangeLogDTO MapProjectLogToDTO(ProjectChangeLog log)
        {
            return new ProjectChangeLogDTO
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
        }

        private TeamChangeLogDTO MapTeamLogToDTO(TeamChangeLog log)
        {
            return new TeamChangeLogDTO
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
        }

        private TaskItemChangeLogDTO MapTaskLogToDTO(TaskItemChangeLog log)
        {
            return new TaskItemChangeLogDTO
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
}
