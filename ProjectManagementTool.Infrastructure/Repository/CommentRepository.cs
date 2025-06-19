using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public async Task<IEnumerable<Comment>> GetAllByTaskIdAsync(Guid taskItemId)
        {
            return await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.TaskItem)
                .Where(c => c.TaskItemId == taskItemId)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.TaskItem)
                .Where(c => c.AuthorId == userId)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
        }

        public Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}