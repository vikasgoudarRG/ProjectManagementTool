using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public class UserNotificationMapper : IUserNotificationMapper
    {
        public UserNotificationDTO ToDTO(UserNotification userNotification)
        {
            return new UserNotificationDTO
            {
                Message = userNotification.Message,
                CreatedOn = userNotification.CreatedOn
            };
        }
    } 
}