using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Team;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        // ========== Team Operations ==========

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

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

        public async Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        // ========== Team Member Operations ==========

        public async Task AddMemberAsync(Guid teamId, Guid userId, TeamMemberRole role)
        {
            var member = new TeamMember(teamId, userId, role);
            await _context.TeamMembers.AddAsync(member);
            await _context.SaveChangesAsync();
        }

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

        public async Task UpdateMemberAsync(TeamMember teamMember)
        {
            _context.TeamMembers.Update(teamMember);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(Guid teamId, Guid userId)
        {
            var member = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
            if (member != null)
            {
                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
        }
    }
}
