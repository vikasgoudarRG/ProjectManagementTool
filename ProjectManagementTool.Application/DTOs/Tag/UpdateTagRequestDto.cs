namespace ProjectManagementTool.Application.DTOs.Tag
{
    public class UpdateTagRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}