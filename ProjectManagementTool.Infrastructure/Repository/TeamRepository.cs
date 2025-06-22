using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Team;
using ProjectManagementTool.Infrastructure.Contexts;
using ProjectManageMentTool.Application.Interfaces.Repositories;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class TeamRepository : ITeamRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructors
        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion Constructors

        #region Methods
        // Team Operations
        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        public async Task<Team?> GetByIdAsync(Guid teamId)
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<Team?> GetByNameAsync(string name, Guid projectId)
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                .FirstOrDefaultAsync(t => t.Name == name && t.ProjectId == projectId);
        }

        public async Task<IEnumerable<Team>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _context.Teams
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.TeamMembers)
                .ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetAllByProjectIdAndUserIdAsync(Guid projectId, Guid userId)
        {
            return await _context.Teams
                .Where(t => t.ProjectId == projectId && t.TeamMembers.Any(m => m.UserId == userId))
                .Include(t => t.TeamMembers)
                .ToListAsync();
        }

        public async Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId)
        {
            return await _context.Teams
                .AnyAsync(t => t.Id == teamId && t.TeamMembers.Any(m => m.UserId == userId));
        }

        public async Task<bool> ExistsTeamByNameAsync(string teamName, Guid projectId)
        {
            return await _context.Teams.AnyAsync(t => t.Name == teamName && t.ProjectId == projectId);
        }

        public Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
            return Task.CompletedTask;
        }


        // Team Member Operations
        public async Task AddMemberAsync(Guid teamId, Guid userId, TeamMemberRole role)
        {
            TeamMember teamMember = new TeamMember
            (
                teamId: teamId,
                userId: userId,
                role: role
            );
            await _context.TeamMembers.AddAsync(teamMember);
        }

        public async Task<TeamMember?> GetMemberAsync(Guid teamId, Guid userId)
        {
            return await _context.TeamMembers
            .Include(tm => tm.User)
            .Include(tm => tm.Team)
            .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
        }

        public async Task<IEnumerable<TeamMember>> GetAllMembersAsync(Guid teamId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Include(tm => tm.User)
                .ToListAsync();
        }

        public Task UpdateMemberAsync(TeamMember teamMember)
        {
            _context.TeamMembers.Update(teamMember);
            return Task.CompletedTask;
        }

        public Task RemoveMemberAsync(TeamMember teamMember)
        {
            _context.TeamMembers.Remove(teamMember);
            return Task.CompletedTask;
        }
    }
    #endregion Methods
}