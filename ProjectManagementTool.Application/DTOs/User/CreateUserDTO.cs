namespace ProjectManagementTool.Application.DTOs.User
{
    public class CreateUserDTO
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}