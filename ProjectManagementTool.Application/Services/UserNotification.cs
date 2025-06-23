using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Common;
using ProjectManagementTool.Application.DTOs.Notification;

namespace ProjectManagementTool.Application.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserNotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserNotificationService(
            IUserNotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserNotificationDto>> GetAllByUserIdAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId);
            return notifications.Select(n =>
                new UserNotificationDto(n.Id, n.UserId, n.Message, n.IsRead, n.CreatedOn));
        }

        public async Task MarkAsReadAndDeleteAsync(Guid notificationId)
        {
            var notif = await _notificationRepository.GetByIdAsync(notificationId);
            if (notif is null) return;

            notif.MarkAsRead();
            await _notificationRepository.DeleteAsync(notif);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task MarkAllAsReadAndDeleteAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId);
            foreach (var notif in notifications)
            {
                notif.MarkAsRead();
                await _notificationRepository.DeleteAsync(notif);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}