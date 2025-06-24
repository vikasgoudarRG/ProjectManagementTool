using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        // ======================= Fields ======================= //
        private readonly AppDbContext _context;

        // ==================== Constructors ==================== //
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // ======================= Methods ====================== //
        // Create
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        // Read
        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchAsync(string keyword)
        {
            keyword = keyword.ToLower();
            return await _context.Users
                .Where(u => u.Name.ToLower().Contains(keyword) || u.Email.ToLower().Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllByProjectIdAsync(Guid projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return Enumerable.Empty<User>();

            return await _context.Users
                .Where(u => project.Developers.Contains(u))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllByTeamIdAsync(Guid teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null) return Enumerable.Empty<User>();

            return await _context.Users
                .Where(u => team.TeamMembers.Any(t => t.UserId == u.Id))
                .ToListAsync();
        }

        // Update
        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        // Delete
        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }
    }

}