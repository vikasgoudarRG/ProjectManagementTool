using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.Notification
{
    public class UserNotificationDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public Guid UserId { get; set; }
        public bool isRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserNotificationDto(Guid id, Guid userId, string message, bool isRead, DateTime createdOn)
        {
            Id = id;
            UserId = userId;
            Message = message;
            this.isRead = isRead;
            CreatedAt = createdOn;
        }
    }
}