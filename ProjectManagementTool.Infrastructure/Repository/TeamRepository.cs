using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Team;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        // ======================= Fields ======================= //
        private readonly AppDbContext _context;

        // ==================== Constructors ==================== //
        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        // ======================= Methods ====================== //
        // =========== Team ===========
        // Create
        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        // Read
        public async Task<Team?> GetByIdAsync(Guid teamId)
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<Team?> GetByNameAsync(string teamName, Guid projectId)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Name == teamName);
        }

        public async Task<IEnumerable<Team>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _context.Teams
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetAllByProjectIdAndUserIdAsync(Guid projectId, Guid userId)
        {
            return await _context.Teams
                .Where(t => t.ProjectId == projectId &&
                            t.TeamMembers.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId)
        {
            return await _context.TeamMembers.AnyAsync(m =>
                m.TeamId == teamId && m.UserId == userId);
        }

        // Update
        public Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            return Task.CompletedTask;
        }

        // Delete
        public Task DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
            return Task.CompletedTask;
        }

        // =========== Team Member ===========
        // Create
        public async Task AddMemberAsync(Guid teamId, Guid userId, TeamMemberRole role)
        {
            var member = new TeamMember(teamId, userId, role);
            await _context.TeamMembers.AddAsync(member);
        }

        // Read
        public async Task<TeamMember?> GetMemberAsync(Guid teamId, Guid userId)
        {
            return await _context.TeamMembers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
        }

        public async Task<IEnumerable<TeamMember>> GetAllMembersAsync(Guid teamId)
        {
            return await _context.TeamMembers
                .Where(m => m.TeamId == teamId)
                .Include(m => m.User)
                .ToListAsync();
        }

        public async Task<bool> IsTeamLeadAsync(Guid teamId, Guid userId)
        {
            return await _context.TeamMembers
                .AnyAsync(m => m.TeamId == teamId && m.UserId == userId && m.Role == TeamMemberRole.Lead);
        }

        // Update
        public Task UpdateMemberAsync(TeamMember teamMember)
        {
            _context.TeamMembers.Update(teamMember);
            return Task.CompletedTask;
        }

        // Delete
        public async Task RemoveMemberAsync(Guid teamId, Guid userId)
        {
            var member = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
            if (member != null)
            {
                _context.TeamMembers.Remove(member);
            }
        }
    }
}
