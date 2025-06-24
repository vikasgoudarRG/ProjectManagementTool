using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class TaskItemCommentRepository : ITaskItemCommentRepository
    {
        private readonly AppDbContext _context;

        public TaskItemCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItemComment comment)
        {
            await _context.TaskItemComments.AddAsync(comment);
        }

        public async Task<TaskItemComment?> GetByIdAsync(Guid commentId)
        {
            return await _context.TaskItemComments
                .Include(c => c.Author)
                .Include(c => c.TaskItem)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<IEnumerable<TaskItemComment>> GetAllByTaskIdAsync(Guid taskItemId)
        {
            return await _context.TaskItemComments
                .Where(c => c.TaskItemId == taskItemId)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItemComment>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.TaskItemComments
                .Where(c => c.AuthorId == userId)
                .Include(c => c.TaskItem)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public Task UpdateAsync(TaskItemComment comment)
        {
            _context.TaskItemComments.Update(comment);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TaskItemComment comment)
        {
            _context.TaskItemComments.Remove(comment);
            return Task.CompletedTask;
        }
    }
}
