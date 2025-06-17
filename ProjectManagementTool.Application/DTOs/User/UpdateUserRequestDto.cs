namespace ProjectManagementTool.Application.DTOs.User
{
    public class UpdateUserRequestDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}