using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Identity.Client;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructors
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion Constructors

        #region Methods
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Teams)
                .Include(p => p.Developers)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<Project?> GetByNameAsync(string name)
        {
            return await _context.Projects
                .Include(p => p.Teams)
                .Include(p => p.Developers)
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Teams)
                .Include(p => p.Developers)
                .Where(p => p.ProjectLeadId == userId || p.Developers.Any(d => d.Id == userId))
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
        #endregion Methods
    }
}