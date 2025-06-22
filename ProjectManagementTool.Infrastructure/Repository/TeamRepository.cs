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
        private readonly AppDbContext _context;
        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        public async Task<Team?> GetByIdAsync(Guid teamId)
        {
            return await _context.Teams.FindAsync(teamId);
        }

        public async Task<Team?> GetByNameAsync(string name, Guid projectId)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(t => t.Name == name && t.ProjectId == projectId);
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
                .Include(t => t.TeamMembers)
                .Where(t => t.ProjectId == projectId && t.TeamMembers.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId)
        {
            return await _context.Teams
                .AnyAsync(t => t.Id == teamId && t.TeamMembers.Any(m => m.UserId == userId));
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
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
        }

        public async Task<IEnumerable<TeamMember>> GetAllMembersAsync(Guid teamId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
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
}