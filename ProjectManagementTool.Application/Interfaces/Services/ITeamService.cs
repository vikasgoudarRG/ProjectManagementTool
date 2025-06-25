using ProjectManagementTool.Application.DTOs.Team;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface ITeamService
    {
        Task<TeamDTO> CreateTeamAsync(CreateTeamDTO dto);
        Task<TeamDTO> GetByIdAsync(Guid teamId, Guid requesterId);
        Task<IEnumerable<TeamDTO>> GetAllByProjectIdAsync(Guid projectId, Guid requesterId);

        Task AddMemberAsync(Guid teamId, AddMemberDTO dto);
        Task RemoveMemberAsync(Guid teamId, Guid userId, Guid requesterIdo);

        Task AssignTeamLeadAsync(Guid teamId, AssignLeadDTO dto);
        Task RemoveTeamLeadAsync(Guid teamId, AssignLeadDTO dto);

        Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId);
        Task<bool> IsTeamLeadAsync(Guid teamId, Guid userId);

        Task DeleteTeamAsync(Guid teamId, Guid requesterId);
    }

}