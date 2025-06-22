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

        public async Task<ProjectChangeLog?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectChangeLogs
                    .Include(l => l.Project)
                    .Include(l => l.ChangedByUser)
                    .FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<IEnumerable<ProjectChangeLog>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _context.ProjectChangeLogs
                .Where(l => l.ProjectId == projectId)
                .Include(l => l.Project)
                .Include(l => l.ChangedByUser)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync(); 
        }
        #endregion Methods
    }
}