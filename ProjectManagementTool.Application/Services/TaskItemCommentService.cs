using ProjectManagementTool.Application.DTOs.Comment;
using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.Interfaces.Mappers;
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
        // ======================= Fields ======================= //
        private readonly ITaskItemRepository _taskRepository;
        private readonly ITaskItemCommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskItemChangeLogRepository _logRepository;
        private readonly IUserNotificationService _notificationService;
        private readonly ITaskItemCommentMapper _taskItemCommentMapper;
        private readonly IUnitOfWork _unitOfWork;

        // ==================== Constructors ==================== //
        public TaskItemCommentService(
            ITaskItemRepository taskRepository,
            ITaskItemCommentRepository commentRepository,
            IUserRepository userRepository,
            ITaskItemChangeLogRepository logRepository,
            IUserNotificationService notificationService,
            ITaskItemCommentMapper taskItemCommentMapper,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _logRepository = logRepository;
            _notificationService = notificationService;
            _taskItemCommentMapper = taskItemCommentMapper;
            _unitOfWork = unitOfWork;
        }

        // ======================= Methods ====================== //
        // Create
        public async Task AddAsync(CreateCommentDTO dto)
        {
            TaskItem task = await _taskRepository.GetByIdAsync(dto.TaskItemId)
                        ?? throw new KeyNotFoundException("Task not found");
            User user = await _userRepository.GetByIdAsync(dto.AuthorId)
                        ?? throw new KeyNotFoundException("User not found");

            TaskItemComment comment = new TaskItemComment(dto.TaskItemId, dto.AuthorId, dto.Text);
            await _commentRepository.AddAsync(comment);

            var log = new TaskItemChangeLog(
                taskItemId: task.Id,
                changedByUserId: user.Id,
                changeType: ChangeType.Created,
                propertyChanged: "Comment",
                oldValue: null,
                newValue: dto.Text
            );
            await _logRepository.AddAsync(log);

            // Notify assignee if different from commenter
            if (task.AssignedUserId.HasValue && task.AssignedUserId != dto.AuthorId)
            {
                UserNotification userNotification = new UserNotification(
                    userId: task.AssignedUserId.Value,
                    message: $"New comment on your task: {task.Title}"
                );
                await _notificationService.SendUserNotification(userNotification);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        // Read
        public async Task<IEnumerable<TaskItemCommentDTO>> GetAllForTaskAsync(Guid taskId)
        {
            if (await _taskRepository.GetByIdAsync(taskId) is null)
                throw new KeyNotFoundException("Task not found");

            IEnumerable<TaskItemComment> comments = await _commentRepository.GetAllByTaskIdAsync(taskId);
            return comments.Select(c => _taskItemCommentMapper.ToDTO(c));
        }

        // Update
        public async Task UpdateAsync(Guid commentId, Guid authorId, string updatedContent)
        {
            TaskItemComment comment = await _commentRepository.GetByIdAsync(commentId)
                           ?? throw new KeyNotFoundException("Comment not found");

            if (comment.AuthorId != authorId)
                throw new UnauthorizedAccessException("Not authorized to edit this comment");

            string oldContent = comment.Text;
            comment.Edit(updatedContent);
            await _commentRepository.UpdateAsync(comment);

            TaskItemChangeLog log = new TaskItemChangeLog(
                taskItemId: comment.TaskItemId,
                changedByUserId: authorId,
                changeType: ChangeType.Updated,
                propertyChanged: "Comment",
                oldValue: oldContent,
                newValue: updatedContent
            );
            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteAsync(Guid commentId, Guid authorId)
        {
            TaskItemComment comment = await _commentRepository.GetByIdAsync(commentId)
                           ?? throw new KeyNotFoundException("Comment not found");

            if (comment.AuthorId != authorId)
                throw new UnauthorizedAccessException("Not authorized to delete this comment");

            string oldContent = comment.Text;
            await _commentRepository.DeleteAsync(comment);

            TaskItemChangeLog log = new TaskItemChangeLog(
                taskItemId: comment.TaskItemId,
                changedByUserId: authorId,
                changeType: ChangeType.Deleted,
                propertyChanged: "Comment",
                oldValue: oldContent,
                newValue: null
            );
            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
