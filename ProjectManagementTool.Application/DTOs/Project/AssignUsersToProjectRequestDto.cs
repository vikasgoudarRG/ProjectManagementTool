namespace ProjectManagementTool.Application.DTOs.Project
{
    public class AssignUsersToProjectRequestDto
    {
        public ICollection<Guid> DeveloperIds { get; set; } = new List<Guid>();
    }
}