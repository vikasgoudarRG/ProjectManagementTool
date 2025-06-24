using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class ProjectChangeLogRepository : IProjectChangeLogRepository
    {
        private readonly AppDbContext _context;

        public ProjectChangeLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProjectChangeLog log)
        {
            await _context.ProjectChangeLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjectChangeLog?> GetByIdAsync(Guid logId)
        {
            return await _context.ProjectChangeLogs
                .Include(l => l.ChangedByUser)
                .Include(l => l.Project)
                .FirstOrDefaultAsync(l => l.Id == logId);
        }

        public async Task<IEnumerable<ProjectChangeLog>> GetAllByProjectIdAsync(
            Guid projectId,
            DateTime? from = null,
            DateTime? to = null,
            ChangeType? type = null)
        {
            var query = _context.ProjectChangeLogs
                .Include(l => l.ChangedByUser)
                .Include(l => l.Project)
                .Where(l => l.ProjectId == projectId)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(l => l.CreatedOn >= from.Value);

            if (to.HasValue)
                query = query.Where(l => l.CreatedOn <= to.Value);

            if (type.HasValue)
                query = query.Where(l => l.ChangeType == type.Value);

            return await query.ToListAsync();
        }
    }
}
