using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
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

        public async Task<TaskItemChangeLog?> GetByIdAsync(Guid logId)
        {
            return await _context.TaskItemChangeLogs
                    .Include(l => l.ChangedByUser)
                    .Include(l => l.TaskItem)
                    .FirstOrDefaultAsync(l => l.Id == logId);
        }

        public async Task<IEnumerable<TaskItemChangeLog>> GetAllByTaskItemIdAsync(Guid taskItemId)
        {
            return await _context.TaskItemChangeLogs
                .Where(l => l.TaskItemId == taskItemId)
                .Include(l => l.ChangedByUser)
                .Include(l => l.TaskItem)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();
        }
    }
}