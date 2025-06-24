using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Exceptions;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemCommentService : ITaskItemCommentService
    {
        private readonly ITaskItemRepository _taskRepository;
        private readonly ITaskItemCommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskItemChangeLogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskItemCommentService(
            ITaskItemRepository taskRepository,
            ITaskItemCommentRepository commentRepository,
            IUserRepository userRepository,
            ITaskItemChangeLogRepository logRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreateCommentDTO dto)
        {
            var task = await _taskRepository.GetByIdAsync(dto.TaskItemId)
                       ?? throw new NotFoundException("Task not found");

            var user = await _userRepository.GetByIdAsync(dto.AuthorId)
                       ?? throw new NotFoundException("User not found");

            var comment = new TaskItemComment(dto.TaskItemId, dto.AuthorId, dto.Text);

            await _commentRepository.AddAsync(comment);

            // Log creation
            var log = new TaskItemChangeLog(
                taskItemId: dto.TaskItemId,
                propertyChanged: "Comment",
                oldValue: null,
                newValue: "Created",
                changeType: ChangeType.Created,
                changedByUserId: dto.AuthorId
            );

            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid commentId, Guid authorId, string updatedContent)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId)
                           ?? throw new NotFoundException("Comment not found");

            if (comment.AuthorId != authorId)
                throw new UnauthorizedAccessException("You can only edit your own comment.");

            var oldContent = comment.Text;

            comment.Edit(updatedContent);

            await _commentRepository.UpdateAsync(comment);

            // Log edit
            var log = new TaskItemChangeLog(
                taskItemId: comment.TaskItemId,
                propertyChanged: "Comment",
                oldValue: oldContent,
                newValue: updatedContent,
                changeType: ChangeType.Updated,
                changedByUserId: authorId
            );

            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId, Guid authorId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId)
                           ?? throw new NotFoundException("Comment not found");

            if (comment.AuthorId != authorId)
                throw new UnauthorizedAccessException("You can only delete your own comment.");

            await _commentRepository.DeleteAsync(comment);

            // Log deletion
            var log = new TaskItemChangeLog(
                taskItemId: comment.TaskItemId,
                propertyChanged: "Comment",
                oldValue: comment.Text,
                newValue: null,
                changeType: ChangeType.Deleted,
                changedByUserId: authorId
            );

            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItemCommentDTO>> GetAllForTaskAsync(Guid taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                       ?? throw new NotFoundException("Task not found");

            var comments = await _commentRepository.GetAllByTaskIdAsync(taskId);
            return comments.Select(MapToDTO);
        }

        private TaskItemCommentDTO MapToDTO(TaskItemComment comment)
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
