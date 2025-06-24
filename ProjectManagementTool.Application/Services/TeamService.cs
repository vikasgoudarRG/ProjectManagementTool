using ProjectManagementTool.Application.DTOs.Team;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.Change;
using ProjectManagementTool.Domain.Enums.Team;
using ProjectManagementTool.Domain.Exceptions;
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

        public TeamService(
            ITeamRepository teamRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IChangeLogService changeLogService)
        {
            _teamRepository = teamRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _changeLogService = changeLogService;
        }

        public async Task<Guid> CreateTeamAsync(CreateTeamDTO dto)
        {
            var project = await _projectRepository.GetByIdAsync(dto.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            var team = new Team(dto.Name, dto.ProjectId);

            await _teamRepository.AddAsync(team);
            await _changeLogService.AddTeamLogAsync(new TeamChangeLog
            {
                TeamId = team.Id,
                ChangedByUserId = project.ProjectLeadId,
                PropertyChanged = "Team",
                OldValue = null,
                NewValue = team.Name,
                ChangeType = ChangeType.Created
            });

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
                          ?? throw new NotFoundException("Project not found");

            if (!project.IsMember(requesterId))
                throw new UnauthorizedAccessException("You are not part of this project.");

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
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can add members.");

            await _teamRepository.AddMemberAsync(dto.TeamId, dto.UserId, ParseRole(dto.Role));

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog
            {
                TeamId = team.Id,
                ChangedByUserId = dto.RequesterId,
                PropertyChanged = "Member",
                OldValue = null,
                NewValue = $"{dto.UserId} as {dto.Role}",
                ChangeType = ChangeType.Added
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(TeamMemberActionDTO dto)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId)
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can remove members.");

            await _teamRepository.RemoveMemberAsync(dto.TeamId, dto.UserId);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog
            {
                TeamId = team.Id,
                ChangedByUserId = dto.RequesterId,
                PropertyChanged = "Member",
                OldValue = dto.UserId.ToString(),
                NewValue = null,
                ChangeType = ChangeType.Removed
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
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can modify leads.");

            var oldRole = assign ? "Developer" : "TeamLead";
            var newRole = assign ? "TeamLead" : "Developer";

            var newMember = new TeamMember(dto.TeamId, dto.UserId, ParseRole(newRole));
            await _teamRepository.UpdateMemberAsync(newMember);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog
            {
                TeamId = team.Id,
                ChangedByUserId = dto.RequesterId,
                PropertyChanged = "TeamLead",
                OldValue = assign ? null : dto.UserId.ToString(),
                NewValue = assign ? dto.UserId.ToString() : null,
                ChangeType = ChangeType.Updated
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(Guid teamId, Guid requesterId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId)
                       ?? throw new NotFoundException("Team not found");

            var project = await _projectRepository.GetByIdAsync(team.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only project leads can delete teams.");

            await _teamRepository.DeleteAsync(team);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog
            {
                TeamId = team.Id,
                ChangedByUserId = requesterId,
                PropertyChanged = "Team",
                OldValue = team.Name,
                NewValue = null,
                ChangeType = ChangeType.Deleted
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId)
        {
            return await _teamRepository.IsUserInTeamAsync(teamId, userId);
        }

        public async Task<bool> IsTeamLeadAsync(Guid teamId, Guid userId)
        {
            var member = await _teamRepository.GetMemberAsync(teamId, userId);
            return member?.Role == TeamMemberRole.TeamLead;
        }

        private TeamMemberRole ParseRole(string role)
        {
            return Enum.TryParse<TeamMemberRole>(role, true, out var parsed)
                ? parsed
                : throw new InvalidEnumException($"Invalid role: {role}");
        }
    }
}
