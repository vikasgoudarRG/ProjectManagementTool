using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemCommentService : ITaskItemCommentService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ITaskItemCommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public TaskItemCommentService(
            ITaskItemRepository taskItemRepository,
            ITaskItemCommentRepository commentRepository,
            IUserRepository userRepository)
        {
            _taskItemRepository = taskItemRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task AddCommentAsync(Guid taskId, Guid authorId, string content)
        {
            var task = await _taskItemRepository.GetByIdAsync(taskId) ?? throw new ArgumentException("Invalid task");
            var user = await _userRepository.GetByIdAsync(authorId) ?? throw new ArgumentException("Invalid user");

            var comment = new TaskItemComment(taskId, authorId, content);
            await _commentRepository.AddAsync(comment);
        }

        public async Task<IEnumerable<TaskItemCommentDTO>> GetCommentsForTaskAsync(Guid taskId)
        {
            var comments = await _commentRepository.GetAllByTaskItemIdAsync(taskId);
            return comments.Select(c => new TaskItemCommentDTO(c.Id, c.AuthorId, c.TaskItemId, c.Content, c.CreatedOn));
        }
    }
}
