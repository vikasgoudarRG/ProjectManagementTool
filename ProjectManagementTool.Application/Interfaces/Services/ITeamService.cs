using ProjectManagementTool.Application.DTOs.Team;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITeamService
    {
        Task<Guid> CreateTeamAsync(CreateTeamDTO dto);
        Task<TeamDTO?> GetByIdAsync(Guid teamId, Guid requesterId);
        Task<IEnumerable<TeamDTO>> GetAllByProjectIdAsync(Guid projectId, Guid requesterId);

        Task AddMemberAsync(TeamMemberActionDTO dto);
        Task RemoveMemberAsync(TeamMemberActionDTO dto);

        Task AssignTeamLeadAsync(TeamMemberActionDTO dto);
        Task RemoveTeamLeadAsync(TeamMemberActionDTO dto);

        Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId);
        Task<bool> IsTeamLeadAsync(Guid teamId, Guid userId);

        Task DeleteTeamAsync(Guid teamId, Guid requesterId);
    }

}