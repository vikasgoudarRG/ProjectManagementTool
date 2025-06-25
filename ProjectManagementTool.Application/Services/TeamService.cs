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
using ProjectManagementTool.Application.Interfaces.Mappers;

namespace ProjectManagementTool.Application.Services
{
    public class TeamService : ITeamService
    {
        // ======================= Fields ======================= //
        private readonly ITeamRepository _teamRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChangeLogService _changeLogService;
        private readonly ITeamMapper _teamMapper;
        private readonly IUserNotificationService _notificationService;

        // ==================== Constructors ==================== //
        public TeamService(
            ITeamRepository teamRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IChangeLogService changeLogService,
            ITeamMapper teamMapper,
            IUserNotificationService notificationService)
        {
            _teamRepository = teamRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _teamMapper = teamMapper;
            _changeLogService = changeLogService;
            _notificationService = notificationService;
        }

        // ======================= Methods ====================== //
        // Create
        public async Task<TeamDTO> CreateTeamAsync(CreateTeamDTO dto)
        {
            Project project = await _projectRepository.GetByIdAsync(dto.ProjectId)
                          ?? throw new KeyNotFoundException("Project not found");
            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Access denied");

            Team team = new Team(dto.Name, dto.ProjectId);
            await _teamRepository.AddAsync(team);

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id, dto.RequesterId, ChangeType.Created, "Team Entity", null, team.Name));

            await _unitOfWork.SaveChangesAsync();
            return _teamMapper.ToDTO(team);
        }

        // Read
        public async Task<TeamDTO> GetByIdAsync(Guid teamId, Guid requesterId)
        {
            Team team = await _teamRepository.GetByIdAsync(teamId) ?? throw new KeyNotFoundException("Team does not exist");
            Project project = await _projectRepository.GetByIdAsync(team.ProjectId) ?? throw new KeyNotFoundException("Project does not exist");
            bool isLead = project.ProjectLeadId == requesterId;
            if (!isLead && !await _teamRepository.IsUserInTeamAsync(teamId, requesterId))
                throw new UnauthorizedAccessException("Access denied.");
            return _teamMapper.ToDTO(team);
        }

        public async Task<IEnumerable<TeamDTO>> GetAllByProjectIdAsync(Guid projectId, Guid requesterId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId)
                          ?? throw new KeyNotFoundException("Project not found");

            if (!project.IsMember(requesterId))
                throw new UnauthorizedAccessException("Access denied.");

            IEnumerable<Team> teams = await _teamRepository.GetAllByProjectIdAsync(projectId);
            return teams.Select(t => _teamMapper.ToDTO(t));
        }

        public async Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId)
        {
            return await _teamRepository.IsUserInTeamAsync(teamId, userId);
        }

        public async Task<bool> IsTeamLeadAsync(Guid teamId, Guid userId)
        {
            TeamMember member = await _teamRepository.GetMemberAsync(teamId, userId) ?? throw new KeyNotFoundException("User not in team");
            return member.Role == TeamMemberRole.Lead;
        }

        // Update
        public async Task AssignTeamLeadAsync(Guid teamId, AssignLeadDTO dto)
        {
            await ChangeTeamLeadAsync(teamId, dto, true);
        }

        public async Task RemoveTeamLeadAsync(Guid teamId, AssignLeadDTO dto)
        {
            await ChangeTeamLeadAsync(teamId, dto, false);
        }

        private async Task ChangeTeamLeadAsync(Guid teamId, AssignLeadDTO dto, bool assign)
        {
            Team team = await _teamRepository.GetByIdAsync(teamId)
                       ?? throw new KeyNotFoundException("Team not found");
            Project project = await _projectRepository.GetByIdAsync(team.ProjectId)
                         ?? throw new KeyNotFoundException("Project not found");
            TeamMember member = await _teamRepository.GetMemberAsync(team.Id, dto.UserId) ?? throw new KeyNotFoundException("User not in team");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can modify team leads.");

            member.UpdateRole(assign ? TeamMemberRole.Lead : TeamMemberRole.Developer);
            await _teamRepository.UpdateMemberAsync(member);
            await _unitOfWork.SaveChangesAsync();

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id, dto.RequesterId, ChangeType.Updated, "TeamLead",
                assign ? null : dto.UserId.ToString(),
                assign ? dto.UserId.ToString() : null));


            UserNotification userNotification = new UserNotification(
                userId: dto.UserId,
                message: assign
                    ? $"You have been assigned as Team Lead for '{team.Name}'."
                    : $"You have been removed as Team Lead for '{team.Name}'."
            );
            await _notificationService.SendUserNotification(userNotification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddMemberAsync(Guid teamId, AddMemberDTO dto)
        {
            Team team = await _teamRepository.GetByIdAsync(teamId)
                       ?? throw new KeyNotFoundException("Team not found");
            Project project = await _projectRepository.GetByIdAsync(team.ProjectId)
                         ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only project leads can add team members.");

            if (!project.IsMember(dto.UserId))
                throw new ArgumentException("User must first be part of the project.");

            TeamMember teamMember = new TeamMember(
                teamId: teamId,
                userId: dto.UserId,
                role: TeamMemberRole.Developer
            );
            team.AddMember(teamMember);
            await _teamRepository.UpdateAsync(team);
            await _unitOfWork.SaveChangesAsync();


            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                teamId, dto.RequesterId, ChangeType.Added, "TeamMember", null, $"{dto.UserId} as Developer"));

            UserNotification userNotification = new UserNotification(
                userId: dto.UserId,
                message: $"You have been added to Team '{team.Name}' as Developer."
            );
            await _notificationService.SendUserNotification(userNotification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(Guid teamId, Guid userId, Guid requesterId)
        {
            Team team = await _teamRepository.GetByIdAsync(teamId)
                       ?? throw new KeyNotFoundException("Team not found");
            Project project = await _projectRepository.GetByIdAsync(team.ProjectId)
                         ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only project leads can remove team members.");

            await _teamRepository.RemoveMemberAsync(teamId, userId);
            await _unitOfWork.SaveChangesAsync();

            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id, requesterId, ChangeType.Removed, "TeamMember", userId.ToString(), null));


            UserNotification userNotification = new UserNotification(
                userId: userId,
                message: $"You have been removed from Team: {team.Id}"
            );
            await _notificationService.SendUserNotification(userNotification);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteTeamAsync(Guid teamId, Guid requesterId)
        {
            Team team = await _teamRepository.GetByIdAsync(teamId)
                       ?? throw new KeyNotFoundException("Team not found");
            Project project = await _projectRepository.GetByIdAsync(team.ProjectId)
                         ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only project leads can delete teams.");
                
            await _changeLogService.AddTeamLogAsync(new TeamChangeLog(
                team.Id, requesterId, ChangeType.Deleted, "Team", team.Name, null));

            await _teamRepository.DeleteAsync(team);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
