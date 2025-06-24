using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
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

        public async Task<TaskItem?> GetByIdAsync(Guid taskItemId)
        {
            return await _context.TaskItems
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .Include(t => t.Comments)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == taskItemId);
        }

        public async Task<IEnumerable<TaskItem>> GetAllByProjectId(Guid projectId)
        {
            return await _context.TaskItems
                .Include(t => t.Team)
                .Where(t => t.Team.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByTeamId(Guid teamId)
        {
            return await _context.TaskItems
                .Where(t => t.TeamId == teamId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByAssignedUserId(Guid userId)
        {
            return await _context.TaskItems
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByProjectIdAndUserId(Guid projectId, Guid userId)
        {
            return await _context.TaskItems
                .Include(t => t.Team)
                .Where(t => t.AssignedUserId == userId && t.Team.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByTeamIdAndUserId(Guid teamId, Guid userId)
        {
            return await _context.TaskItems
                .Where(t => t.TeamId == teamId && t.AssignedUserId == userId)
                .ToListAsync();
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
    }
}
