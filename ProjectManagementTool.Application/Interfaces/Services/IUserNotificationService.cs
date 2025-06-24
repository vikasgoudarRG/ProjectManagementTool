using ProjectManagementTool.Application.DTOs.Notification;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserNotificationService
    {
        Task SendAsync(SendNotificationDTO dto);
        Task<IEnumerable<UserNotificationDto>> GetAllAsync(Guid userId);
        Task MarkAllAsReadAsync(Guid userId); 
    }

}