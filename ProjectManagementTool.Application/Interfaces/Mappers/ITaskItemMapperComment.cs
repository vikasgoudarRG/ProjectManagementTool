using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Mappers
{
    public interface ITaskItemCommentMapper
    {
        public TaskItemCommentDTO ToDTO(TaskItemComment comment);
    }
}