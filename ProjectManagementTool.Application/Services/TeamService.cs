using ProjectManagementTool.Application.DTOs.Team;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Enums.Team;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;

namespace ProjectManagementTool.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChangeLogService _changeLogService;
        private readonly IUserNotificationService _notificationService;

        public TeamService(
            ITeamRepository teamRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IChangeLogService changeLogService,
            IUserNotificationService notificationService)
        {
            _teamRepository = teamRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _changeLogService = changeLogService;
            _notificationService = notificationService;
        }

        public async Task<Guid> CreateTeamAsync(CreateTeamDTO dto)
        {
            var project = await _projectRepository.GetByIdAsync(dto.ProjectId)
                          ?? throw new ArgumentException("Project not found");

            var team = new Team(dto.Name, dto.ProjectId);
            await _teamRepository.AddAsync(team);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id,
                project.ProjectLeadId,
                ChangeType.Created,
                "Team",
                null,
                team.Name
            ));

            await _unitOfWork.SaveChangesAsync();
            return team.Id;
        }

        public async Task<TeamDTO?> GetByIdAsync(Guid teamId, Guid requesterId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null) return null;

            if (!await _teamRepository.IsUserInTeamAsync(teamId, requesterId))
                throw new UnauthorizedAccessException("Access denied.");

            return new TeamDTO
            {
                Id = team.Id,
                Name = team.Name,
                ProjectId = team.ProjectId,
                CreatedOn = team.CreatedOn
            };
        }

        public async Task<IEnumerable<TeamDTO>> GetAllByProjectIdAsync(Guid projectId, Guid requesterId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                          ?? throw new ArgumentException("Project not found");

            if (!project.IsMember(requesterId))
                throw new UnauthorizedAccessException("Access denied.");

            var teams = await _teamRepository.GetAllByProjectIdAsync(projectId);

            return teams.Select(team => new TeamDTO
            {
                Id = team.Id,
                Name = team.Name,
                ProjectId = team.ProjectId,
                CreatedOn = team.CreatedOn
            });
        }

        public async Task AddMemberAsync(TeamMemberActionDTO dto)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId)
                       ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can add team members.");

            if (!project.IsMember(dto.UserId))
                throw new ArgumentException("User must first be part of the project.");

            await _teamRepository.AddMemberAsync(dto.TeamId, dto.UserId, ParseRole(dto.Role));

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id,
                dto.RequesterId,
                ChangeType.Added,
                "TeamMember",
                null,
                $"{dto.UserId} as {dto.Role}"
            ));

            await _notificationService.SendAsync(new SendNotificationDTO
            {
                UserId = dto.UserId,
                Message = $"You have been added to Team '{team.Name}' as {dto.Role}."
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(TeamMemberActionDTO dto)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId)
                       ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can remove team members.");

            await _teamRepository.RemoveMemberAsync(dto.TeamId, dto.UserId);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id,
                dto.RequesterId,
                ChangeType.Removed,
                "TeamMember",
                dto.UserId.ToString(),
                null
            ));

            await _notificationService.SendAsync(new SendNotificationDTO
            {
                UserId = dto.UserId,
                Message = $"You have been removed from Team '{team.Name}'."
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssignTeamLeadAsync(TeamMemberActionDTO dto)
        {
            await ChangeTeamLeadAsync(dto, assign: true);
        }

        public async Task RemoveTeamLeadAsync(TeamMemberActionDTO dto)
        {
            await ChangeTeamLeadAsync(dto, assign: false);
        }

        private async Task ChangeTeamLeadAsync(TeamMemberActionDTO dto, bool assign)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId)
                       ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can modify team leads.");

            var newRole = assign ? TeamMemberRole.Lead : TeamMemberRole.Developer;

            var updatedMember = new TeamMember(dto.TeamId, dto.UserId, newRole);
            await _teamRepository.UpdateMemberAsync(updatedMember);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id,
                dto.RequesterId,
                ChangeType.Updated,
                "TeamLead",
                assign ? null : dto.UserId.ToString(),
                assign ? dto.UserId.ToString() : null
            ));

            await _notificationService.SendAsync(new SendNotificationDTO
            {
                UserId = dto.UserId,
                Message = assign
                    ? $"You have been assigned as Team Lead for '{team.Name}'."
                    : $"You have been removed as Team Lead for '{team.Name}'."
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(Guid teamId, Guid requesterId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId)
                       ?? throw new ArgumentException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only project leads can delete teams.");

            await _teamRepository.DeleteAsync(team);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id,
                requesterId,
                ChangeType.Deleted,
                "Team",
                team.Name,
                null
            ));

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId)
        {
            return await _teamRepository.IsUserInTeamAsync(teamId, userId);
        }

        public async Task<bool> IsTeamLeadAsync(Guid teamId, Guid userId)
        {
            var member = await _teamRepository.GetMemberAsync(teamId, userId);
            return member?.Role == TeamMemberRole.Lead;
        }

        private TeamMemberRole ParseRole(string role)
        {
            return Enum.TryParse<TeamMemberRole>(role, true, out var parsed)
                ? parsed
                : throw new ArgumentException($"Invalid role: {role}");
        }
    }
}
