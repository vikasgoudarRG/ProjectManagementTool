namespace ProjectManagementTool.Application.DTOs.Notification
{
    public class UserNotificationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ReadOn { get; set; }
    }
}