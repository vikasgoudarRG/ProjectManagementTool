using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class ChangeLogRepository : IChangeLogRepository
    {
        private readonly AppDbContext _context;
        public ChangeLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItemChangeLog log)
        {
            await _context.TaskItemChangeLogs.AddAsync(log);
        }

        public async Task<IEnumerable<TaskItemChangeLog>> GetAllByTaskItemId(Guid taskItemId)
        {
            return await _context.TaskItemChangeLogs
                .Include(l => l.TaskItem)
                .Include(l => l.ChangedByUser)
                .Where(l => l.TaskItemId == taskItemId)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();
        }

        public Task UpdateAsync(TaskItemChangeLog taskItemChangeLog)
        {
            _context.TaskItemChangeLogs.Update(taskItemChangeLog);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TaskItemChangeLog taskItemChangeLog)
        {
            _context.TaskItemChangeLogs.Remove(taskItemChangeLog);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}