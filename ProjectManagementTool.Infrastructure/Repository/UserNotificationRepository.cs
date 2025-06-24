using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.Infrastructure.Repositories
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        #region Fields
        private readonly AppDbContext _context;
        #endregion Fields

        #region Constructor
        public UserNotificationRepository(AppDbContext context)
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

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notification = await _context.UserNotifications.FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                throw new InvalidOperationException("Notification not found");

            if (!notification.IsRead)
            {
                notification.MarkAsRead();
                await _context.SaveChangesAsync();
            }
        }


        public Task DeleteAsync(UserNotification notiication)
        {
            _context.UserNotifications.Remove(notiication);
            return Task.CompletedTask;
        }

        public async Task DeleteAllReadByUserIdAsync(Guid userId)
        {
            var readNotifications = await _context.UserNotifications
                .Where(n => n.UserId == userId && n.IsRead)
                .ToListAsync();

            if (readNotifications.Any())
            {
                _context.UserNotifications.RemoveRange(readNotifications);
                await _context.SaveChangesAsync();
            }
        }
        #endregion Methods
    }
}