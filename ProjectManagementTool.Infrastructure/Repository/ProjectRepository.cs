using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Developers)
                .Include(p => p.TaskItems)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Developers)
                .Include(p => p.TaskItems)
                .ToListAsync();
        }
        public async Task<IEnumerable<Project>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Developers)
                .Include(p => p.TaskItems)
                .Include(p => p.Manager)
                .Where(p => p.Developers.Any(d => d.Id == userId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllByFilterAsync(ProjectFilterQueryModel queryModel)
        {
            IQueryable<Project> query = _context.Projects
                .Include(p => p.Developers)
                .Include(p => p.TaskItems)
                .Include(p => p.Manager);

            if (!string.IsNullOrWhiteSpace(queryModel.TitleKeyword))
            {
                query = query.Where(p => p.Title.Contains(queryModel.TitleKeyword));
            }

            if (queryModel.ManagerId != null)
            {
                query = query.Where(p => p.ManagerId == queryModel.ManagerId);
            }

            if (queryModel.Status != null)
            {
                query = query.Where(p => p.Status == queryModel.Status);
            }

            if (queryModel.DeveloperIds != null && queryModel.DeveloperIds.Any())
            {
                query = query.Where(p => p.Developers.Any(d => queryModel.DeveloperIds.Contains(d.Id)));
                
            }

            if (queryModel.CreatedBefore != null)
            {
                query = query.Where(p => p.CreatedOn < queryModel.CreatedBefore);
            }

            if (queryModel.CreatedAfter != null)
            {
                query = query.Where(p => p.CreatedOn > queryModel.CreatedAfter);
            }

            return await query.ToListAsync();
        }
        public Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}