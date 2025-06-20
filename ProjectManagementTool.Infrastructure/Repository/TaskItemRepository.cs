using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class TaskItemRepository : ITaskItemRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructors
        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion Constructors

        #region Methods
        public async Task AddAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
        }
        public async Task<TaskItem?> GetByIdAsync(Guid taskItemId)
        {
            return await _context.TaskItems
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .Include(t => t.ChangeLogs)
                .FirstOrDefaultAsync(t => t.Id == taskItemId);
        }

        public async Task<IEnumerable<TaskItem>> GetAllByProjectId(Guid projectId)
        {
            return await _context.TaskItems
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.AssignedUser)
                .Include(t => t.Tags)
                .Include(t => t.ChangeLogs)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByUserId(Guid userId)
        {
            return await _context.TaskItems
                .Where(t => t.AssignedUserId == userId)
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .Include(t => t.ChangeLogs)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllTaskItemsByFilter(TaskItemFilterQueryModel queryModel)
        {
            IQueryable<TaskItem> query = _context.TaskItems
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Include(t => t.Tags)
                .Include(t => t.ChangeLogs);

            if (queryModel.ProjectId != null)
            {
                query = query.Where(t => t.ProjectId == queryModel.ProjectId);
            }

            if (queryModel.AssignedUserId != null)
            {
                query = query.Where(t => t.AssignedUserId == queryModel.AssignedUserId);
            }

            if (queryModel.Type != null)
            {
                query = query.Where(t => t.Type == queryModel.Type);
            }

            if (queryModel.Priority != null)
            {
                query = query.Where(t => t.Priority == queryModel.Priority);
            }

            if (queryModel.Status != null)
            {
                query = query.Where(t => t.Status == queryModel.Status);
            }

            if (queryModel.TagIds != null && queryModel.TagIds.Any())
            {
                query = query.Where(t => t.Tags.Any(tag => queryModel.TagIds.Contains(tag.Id)));
            }


            if (queryModel.DeadlineBefore != null)
            {
                query = query.Where(t => (t.Deadline != null) && t.Deadline < queryModel.DeadlineBefore);
            }

            if (queryModel.DeadlineAfter != null)
            {
                query = query.Where(t => (t.Deadline != null) && t.Deadline > queryModel.DeadlineAfter);
            }

            return await query.ToListAsync();
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion Methods
    }
}