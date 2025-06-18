using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskTemRepository;
        public TaskItemService(ITaskItemRepository taskItemRepository)
        {
            _taskTemRepository = taskItemRepository;
        }

        public async Task<Guid> CreateTaskItemAsync(Create)
    }
}