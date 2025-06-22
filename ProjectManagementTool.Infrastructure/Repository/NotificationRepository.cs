using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repository
{
    public class NotificationRepository : IUserNotificationRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructor
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion Constructor

        #region Methods
        public async Task AddAsync(UserNotification notification)
        {
            await _context.UserNotifications.AddAsync(notification);
        }

        public async Task<UserNotification?> GetByIdAsync(Guid notificationId)
        {
            return await _context.UserNotifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);
        }

        public async Task<IEnumerable<UserNotification>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.UserNotifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();
        }

        public Task DeleteAsync(UserNotification notiication)
        {
            _context.UserNotifications.Remove(notiication);
            return Task.CompletedTask;
        }
        #endregion Methods
    }
}