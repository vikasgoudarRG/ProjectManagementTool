using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }
        public Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            return Task.CompletedTask;
        }
        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Developers)
                .Include(p => p.TaskItems)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<ICollection<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Developers)
                .Include(p => p.TaskItems)
                .ToListAsync();
        }
        public async Task<ICollection<Project>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Projects
                .Where(p => p.Developers.Any(d => d.Id == userId))
                .Include(p => p.Developers)
                .Include(p => p.TaskItems)
                .ToListAsync();
        }  

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}