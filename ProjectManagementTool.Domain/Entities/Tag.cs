namespace ProjectManagementTool.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}