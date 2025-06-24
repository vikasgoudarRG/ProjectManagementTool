using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
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

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.ProjectLead)
                .Include(p => p.Teams)
                .Include(p => p.Developers)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<Project?> GetByNameAsync(string projectName)
        {
            return await _context.Projects
                .Include(p => p.ProjectLead)
                .FirstOrDefaultAsync(p => p.Name == projectName);
        }

        public async Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Projects
                .Where(p => p.ProjectLeadId == userId)
                .Include(p => p.Teams)
                .ToListAsync();
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
    }
}
