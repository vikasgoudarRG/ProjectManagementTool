using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.DTOs.Notification
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public UserDto User { get; set; } = null!;
        public Guid? ProjectId { get; set; }
        public Guid? TaskItemId { get; set; }
        public bool isRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}