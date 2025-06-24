using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
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
        private readonly IUserNotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        public TaskItemCommentService(
            ITaskItemRepository taskRepository,
            ITaskItemCommentRepository commentRepository,
            IUserRepository userRepository,
            ITaskItemChangeLogRepository logRepository,
            IUserNotificationService notificationService,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _logRepository = logRepository;
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreateCommentDTO dto)
        {
            var task = await _taskRepository.GetByIdAsync(dto.TaskItemId)
                        ?? throw new ArgumentException("Task not found");
            var user = await _userRepository.GetByIdAsync(dto.AuthorId)
                        ?? throw new ArgumentException("User not found");

            var comment = new TaskItemComment(dto.TaskItemId, dto.AuthorId, dto.Text);
            await _commentRepository.AddAsync(comment);

            var log = new TaskItemChangeLog(
                task.Id,
                user.Id,
                ChangeType.Created,
                "Comment",
                null,
                dto.Text
            );
            await _logRepository.AddAsync(log);

            // Notify assignee if different from commenter
            if (task.AssignedUserId.HasValue && task.AssignedUserId != dto.AuthorId)
            {
                await _notificationService.SendAsync(new SendNotificationDTO
                {
                    UserId = task.AssignedUserId.Value,
                    Message = $"New comment on your task: '{task.Title}'"
                });
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid commentId, Guid authorId, string updatedContent)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId)
                           ?? throw new ArgumentException("Comment not found");

            if (comment.AuthorId != authorId)
                throw new UnauthorizedAccessException("You can only edit your own comment.");

            var oldContent = comment.Text;
            comment.Edit(updatedContent);
            await _commentRepository.UpdateAsync(comment);

            var log = new TaskItemChangeLog(
                comment.TaskItemId,
                authorId,
                ChangeType.Updated,
                "Comment",
                oldContent,
                updatedContent
            );
            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId, Guid authorId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId)
                           ?? throw new ArgumentException("Comment not found");

            if (comment.AuthorId != authorId)
                throw new UnauthorizedAccessException("You can only delete your own comment.");

            var oldContent = comment.Text;
            await _commentRepository.DeleteAsync(comment);

            var log = new TaskItemChangeLog(
                comment.TaskItemId,
                authorId,
                ChangeType.Deleted,
                "Comment",
                oldContent,
                null
            );
            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItemCommentDTO>> GetAllForTaskAsync(Guid taskId)
        {
            if (await _taskRepository.GetByIdAsync(taskId) is null)
                throw new ArgumentException("Task not found");

            var comments = await _commentRepository.GetAllByTaskIdAsync(taskId);
            return comments.Select(MapToDTO);
        }

        private static TaskItemCommentDTO MapToDTO(TaskItemComment comment) => new TaskItemCommentDTO
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
