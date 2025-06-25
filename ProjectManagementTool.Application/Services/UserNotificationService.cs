using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Mappers;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;

namespace ProjectManagementTool.Application.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserNotificationRepository _notificationRepository;
        private readonly IUserNotificationMapper _userNotificationMapper;

        public UserNotificationService(IUserNotificationRepository notificationRepository, IUserNotificationMapper userNotificationMapper)
        {
            _notificationRepository = notificationRepository;
            _userNotificationMapper = userNotificationMapper;
        }

        public async Task SendUserNotification(UserNotification userNotification)
        {
            await _notificationRepository.AddAsync(userNotification);
        }

        public async Task<IEnumerable<UserNotificationDTO>> GetAllAsync(Guid userId)
        {
            IEnumerable<UserNotification> notifications = await _notificationRepository.GetAllByUserIdAsync(userId);
            return notifications.Select(n => _userNotificationMapper.ToDTO(n));
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            IEnumerable<UserNotification> notifications = await _notificationRepository.GetAllByUserIdAsync(userId);

            foreach (UserNotification n in notifications.Where(n => !n.IsRead))
            {
                await _notificationRepository.MarkAsReadAsync(n.Id);
            }
            await _notificationRepository.DeleteAllReadByUserIdAsync(userId);
        }
    }
}
