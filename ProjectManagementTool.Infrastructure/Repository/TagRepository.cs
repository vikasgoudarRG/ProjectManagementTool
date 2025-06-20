using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async Task AddRangeAsync(IEnumerable<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                await AddAsync(tag);
            }
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags
            .ToListAsync();
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags
            .FirstOrDefaultAsync(t => t.Name == name);
        }

        public Task UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Tag tag)
        {
            _context.Tags.Remove(tag);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                DeleteAsync(tag);
            }
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}