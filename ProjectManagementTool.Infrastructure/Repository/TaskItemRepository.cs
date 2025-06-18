using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;
        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
        }

        public Task UpdateAsync(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TaskItem taskItem)
        {
            _context.TaskItems.Remove(taskItem);
            return Task.CompletedTask;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid taskItemId)
        {
            return await _context.TaskItems
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .Include(t => t.ChangeLogs)
                .FirstOrDefaultAsync(t => t.Id == taskItemId);
        }

        public async Task<ICollection<TaskItem>> GetAllByProjectId(Guid projectId)
        {
            return await _context.TaskItems
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .Include(t => t.ChangeLogs)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}