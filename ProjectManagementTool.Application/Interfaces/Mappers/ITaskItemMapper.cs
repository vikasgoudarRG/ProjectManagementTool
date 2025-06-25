using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Mappers
{
    public interface ITaskItemMapper
    {
        public TaskItemDTO ToDTO(TaskItem taskItem);
    }
}