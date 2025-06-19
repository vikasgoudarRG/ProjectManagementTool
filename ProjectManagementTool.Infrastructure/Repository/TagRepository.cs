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
            if (!_context.Tags.Any(t => t.Name == tag.Name))
            {
                await _context.Tags.AddAsync(tag);
            }
        }

        public async Task AddManyAsync(ICollection<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                await AddAsync(tag);
            }
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

        public async Task<ICollection<Tag>> GetOrCreateManyAsync(ICollection<string> names)
        {
            ICollection<Tag> tags = new List<Tag>();
            foreach (string name in names)
            {
                tags.Add(await GetOrCreateAsync(name));
            }
            return tags;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}