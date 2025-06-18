namespace ProjectManagementTool.Application.DTOs.Notification
{
    public class CreateNotificationRequestDto
    {
        public Guid UserID { get; set; }
        public string Message { get; set; } = null!;
    }
}