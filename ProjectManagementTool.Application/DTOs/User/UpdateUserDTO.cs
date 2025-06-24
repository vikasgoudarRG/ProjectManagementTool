namespace ProjectManagementTool.Application.DTOs.User
{

    public class UpdateUserDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}