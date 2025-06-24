using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;

namespace ProjectManagementTool.Application.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserNotificationRepository _notificationRepository;

        public UserNotificationService(IUserNotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task SendAsync(SendNotificationDTO dto)
        {
            var notification = new UserNotification(dto.UserId, dto.Message);
            await _notificationRepository.AddAsync(notification);
        }

        public async Task<IEnumerable<UserNotificationDto>> GetAllAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId);
            return notifications.Select(n => new UserNotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedOn = n.CreatedOn,
                ReadOn = n.ReadOn
            });
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId);

            foreach (var n in notifications.Where(n => !n.IsRead))
            {
                await _notificationRepository.MarkAsReadAsync(n.Id);
            }
            await _notificationRepository.DeleteAllReadByUserIdAsync(userId);
        }
    }
}
