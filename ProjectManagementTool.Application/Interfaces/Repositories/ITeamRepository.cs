using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Team;

namespace ProjectManageMentTool.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        // Team Operations
        Task AddAsync(Team team);

        Task<Team?> GetByIdAsync(Guid teamId);
        Task<Team?> GetByNameAsync(string teamName, Guid projectId);
        Task<IEnumerable<Team>> GetAllByProjectIdAsync(Guid projectId); // for project leads
        Task<IEnumerable<Team>> GetAllByProjectIdAndUserIdAsync(Guid projectId, Guid userId); // for team members
        Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId);

        Task UpdateAsync(Team team);

        Task DeleteAsync(Team team);


        // Team Member Operations
        Task AddMemberAsync(Guid teamId, Guid userId, TeamMemberRole role);
    
        Task<TeamMember?> GetMemberAsync(Guid teamId, Guid userId);
        Task<IEnumerable<TeamMember>> GetAllMembersAsync(Guid teamId); // for project leads and team leads

        Task UpdateMemberAsync(TeamMember teamMember);

        Task RemoveMemberAsync(TeamMember teamMember);

    }
}