using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(CreateNotificationRequestDto dto);
        Task<ICollection<Notification>> GetUnseenNotificationsAsync(Guid userId);
        Task MarkNotificationsAsSeenAsync(Guid userId);
    }
}