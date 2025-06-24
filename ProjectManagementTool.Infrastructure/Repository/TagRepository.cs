using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> GetOrCreateAsync(string name)
        {
            name = name.Trim();
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);

            if (tag != null)
                return tag;

            var newTag = new Tag(name);
            await _context.Tags.AddAsync(newTag);
            return newTag;
        }

        public async Task<Tag?> GetByIdAsync(Guid tagId)
        {
            return await _context.Tags.FindAsync(tagId);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            name = name.Trim();
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
        }

        public Task DeleteAsync(Tag tag)
        {
            _context.Tags.Remove(tag);
            return Task.CompletedTask;
        }
    }
}
