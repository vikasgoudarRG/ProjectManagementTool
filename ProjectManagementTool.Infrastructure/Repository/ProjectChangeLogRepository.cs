using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class ProjectChangeLogRepository : IProjectChangeLogRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructor
        public ProjectChangeLogRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion Constructor

        #region Methods
        public async Task AddAsync(ProjectChangeLog log)
        {
            await _context.ProjectChangeLogs.AddAsync(log);
        }

        public async Task<ProjectChangeLog?> GetById(Guid id)
        {
            return await _context.ProjectChangeLogs.Include(l => l.ChangedByUser)
                                                   .FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<IEnumerable<ProjectChangeLog>> GetAllByProjectId(Guid projectId)
        {
            return await _context.ProjectChangeLogs
                .Include(l => l.ChangedByUser)
                .Where(l => l.ProjectId == projectId)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync(); 
        }
        public Task UpdateAsync(ProjectChangeLog projectChangeLog)
        {
            _context.ProjectChangeLogs.Update(projectChangeLog);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(ProjectChangeLog projectChangeLog)
        {
            _context.ProjectChangeLogs.Remove(projectChangeLog);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion Methods
    }
}