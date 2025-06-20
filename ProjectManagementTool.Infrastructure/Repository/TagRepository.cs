using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class TagRepository : ITagRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructors
        public TagRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion Constructors

        #region Methods
        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async Task AddRangeAsync(IEnumerable<Tag> tags)
        {
            await _context.Tags.AddRangeAsync(tags);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
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
            _context.RemoveRange(tags);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion Methods
    }
}