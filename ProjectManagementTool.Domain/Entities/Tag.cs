namespace ProjectManagementTool.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<TaskItem> Tasks = new List<TaskItem>();
    }
}