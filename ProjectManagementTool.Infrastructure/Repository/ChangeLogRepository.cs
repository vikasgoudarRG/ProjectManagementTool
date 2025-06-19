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

        public async Task<ICollection<TaskItemChangeLog>> GetByTaskItemId(Guid taskItemId)
        {
            return await _context.TaskItemChangeLogs
                .Where(l => l.TaskItemId == taskItemId)
                .OrderByDescending(l => l.ChangedAt)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}