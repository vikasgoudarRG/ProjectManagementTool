using ProjectManagementTool.Application.DTOs.Tag;

namespace ProjectManagementTool.Application.DTOs.TaskItem
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;

        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;

        public Guid? AssignedUser { get; set; }
        public string? AssignedUsername { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }

        public ICollection<TagDto> Tags { get; set; } = new List<TagDto>();

    }
}