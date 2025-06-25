using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserNotificationService
    {
        Task SendUserNotification(UserNotification userNotification);
        Task<IEnumerable<UserNotificationDTO>> GetAllAsync(Guid userId);
        Task MarkAllAsReadAsync(Guid userId); 
    }

}