using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public class TaskItemCommentMapper : ITaskItemCommentMapper
    {
        public TaskItemCommentDTO ToDTO(TaskItemComment comment)
        {
            return new TaskItemCommentDTO
            {
                Id = comment.Id,
                AuthorId = comment.AuthorId,
                Text = comment.Text,
                CreatedOn = comment.CreatedOn,
                Edited = comment.Edited,
                LastEditedOn = comment.LastEditedOn
            };
        }
    }
}