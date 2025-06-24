namespace ProjectManagementTool.Application.DTOs.Notification
{
    public class SendNotificationDTO
    {
        public Guid UserId { get; set; }

        public string Message { get; set; } = null!;
    }
}
