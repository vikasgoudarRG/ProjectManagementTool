namespace ProjectManagementTool.Application.DTOs.Project
{
    public class ProjectUserActionDTO
    {
        public Guid ProjectId { get; set; }

        public Guid DeveloperId { get; set; }

        public Guid RequestingUserId { get; set; }
    }
}
