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
                .Include(t => t.Team.Project)   
                .Include(t => t.Team)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == taskItemId);
        }
        public async Task<IEnumerable<TaskItem>> GetAllByProjectId(Guid projectId)
        {
            return await _context.TaskItems
                .Where(t => t.Team.ProjectId == projectId)
                .Include(t => t.Team.Project)
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByTeamId(Guid teamId)
        {
            return await _context.TaskItems
                .Where(t => t.TeamId == teamId)
                .Include(t => t.Team.Project)
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByAssignedUserId(Guid userId)
        {
            return await _context.TaskItems
                .Where(t => t.AssignedUserId == userId)
                .Include(t => t.Team.Project)
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByProjectIdAndUserId(Guid projectId, Guid userId)
        {
            return await _context.TaskItems
                .Where(t => t.Team.ProjectId == projectId && t.AssignedUserId == userId)
                .Include(t => t.Team.Project)
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByTeamIdAndUserId(Guid teamId, Guid userId)
        {
            return await _context.TaskItems
                .Where(t => t.TeamId == teamId && t.AssignedUserId == userId)
                .Include(t => t.Team.Project)
                .Include(t => t.AssignedUser)
                .Include(t => t.Team)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                .ToListAsync();
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
        #endregion Methods
    }
}