using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public interface IUserNotificationMapper
    {
        public UserNotificationDTO ToDTO(UserNotification userNotification);
    }
}