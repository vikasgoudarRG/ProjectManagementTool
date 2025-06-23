using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
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
            string cleaned = name.Trim().ToLower();    

            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == cleaned);

            if (existingTag != null)
                return existingTag;

            var newTag = new Tag(name);
            _context.Tags.Add(newTag);
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
            string cleaned = name.Trim().ToLower();    
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == cleaned);
        }

        public async Task DeleteAsync(Tag tag)
        {
            _context.Tags.Remove(tag);
            await Task.CompletedTask;
        }
    }
}