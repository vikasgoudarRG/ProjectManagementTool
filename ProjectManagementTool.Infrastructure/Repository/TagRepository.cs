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

        public async Task AddAsync(string name)
        {
            Tag? existing = await GetByNameAsync(name);
            if (existing != null)
                return;

            Tag tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            await _context.Tags.AddAsync(tag);
        }

        public async Task<ICollection<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<Tag> GetOrCreateAsync(string name)
        {
            Tag? existing = await GetByNameAsync(name);
            if (existing != null)
                return (Tag) existing;

            Tag tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            await _context.Tags.AddAsync(tag);
            return tag;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}