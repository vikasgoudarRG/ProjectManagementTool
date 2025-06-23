using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserNotificationService
    {
        public interface IUserNotificationService
    {
        Task<IEnumerable<UserNotificationDto>> GetAllByUserIdAsync(Guid userId);
        Task MarkAsReadAndDeleteAsync(Guid notificationId);
        Task MarkAllAsReadAndDeleteAsync(Guid userId);
    }
    }
}