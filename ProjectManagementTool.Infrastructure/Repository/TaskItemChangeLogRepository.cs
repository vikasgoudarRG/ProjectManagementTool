using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class TaskItemChangeLogRepository : ITaskItemChangeLogRepository
    {
        private readonly AppDbContext _context;

        public TaskItemChangeLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItemChangeLog log)
        {
            await _context.TaskItemChangeLogs.AddAsync(log);
        }

        public async Task<TaskItemChangeLog?> GetByIdAsync(Guid id)
        {
            return await _context.TaskItemChangeLogs
                .Include(l => l.TaskItem)
                .Include(l => l.ChangedByUser)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<TaskItemChangeLog>> GetAllByTaskItemIdAsync(
            Guid taskItemId,
            DateTime? from = null,
            DateTime? to = null,
            ChangeType? type = null)
        {
            var query = _context.TaskItemChangeLogs
                .Where(l => l.TaskItemId == taskItemId)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(l => l.CreatedOn >= from.Value);

            if (to.HasValue)
                query = query.Where(l => l.CreatedOn <= to.Value);

            if (type.HasValue)
                query = query.Where(l => l.ChangeType == type.Value);

            return await query
                .Include(l => l.ChangedByUser)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();
        }
    }
}
