using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Mappers;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.TaskItem;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChangeLogRepository _changeLogRespository;
        public TaskItemService(ITaskItemRepository taskItemRepository, ITagRepository tagRepository, IUserRepository userRepository, IChangeLogRepository changeLogRepository)
        {
            _taskItemRepository = taskItemRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _changeLogRespository = changeLogRepository;
        }

        public async Task<TaskItemDto> CreateTaskItemAsync(CreateTaskItemDto dto)
        {
            User? user = dto.AssignedUserId != null
                ? await _userRepository.GetByIdAsync((Guid) dto.AssignedUserId)
                    ?? throw new Exception($"UserId {dto.AssignedUserId} not found")
                : null;

            ICollection <Tag> tags = new List<Tag>();
            if (dto.Tags != null)
            {
                // check if tag exists before adding
                foreach (string tagName in dto.Tags)
                {
                    Tag? tag = await _tagRepository.GetByNameAsync(tagName);

                    if (tag == null)
                    {
                        tag = new Tag(tagName);
                        await _tagRepository.AddAsync(tag);
                    }
                    tags.Add(tag);
                }
                await _tagRepository.SaveChangesAsync();  
            }


            TaskItem taskItem = new TaskItem(
                title: dto.Title,
                description: dto.Description, 
                type: Enum.TryParse<TaskItemType>(dto.Type, ignoreCase: true, out TaskItemType type) ? type : throw new Exception($"TaskItemType {dto.Type} is invalid"),
                priority: Enum.TryParse<TaskItemPriority>(dto.Priority, ignoreCase: true, out TaskItemPriority priority) ? priority : throw new Exception($"TaskItemPriority {dto.Priority} is invalid"),
                status: Enum.TryParse<TaskItemStatus>(dto.Status, ignoreCase: true, out TaskItemStatus status) ? status : throw new Exception($"TaskItemStatus {dto.Status} is invalid"),
                projectId: dto.ProjectId,
                assignedUserId: dto.AssignedUserId,
                deadline: dto.Deadline,
                tags: tags
            );

            await _taskItemRepository.AddAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();

            return TaskItemMapper.ToDto(taskItem);
        }

        public async Task<TaskItemDto> GetTaskItemById(Guid taskItemId)
        {
            TaskItem taskItem = (TaskItem)(await _taskItemRepository.GetByIdAsync(taskItemId) ?? throw new Exception($"TaskId {taskItemId} not found"));
            return TaskItemMapper.ToDto(taskItem);
        }

        public async Task<IEnumerable<TaskItemDto>> GetAllTaskItemsByProject(Guid projectId)
        {
            IEnumerable<TaskItem> taskItems = await _taskItemRepository.GetAllByProjectId(projectId) ?? throw new Exception($"TaskId {projectId} not found");
            return taskItems.Select(t => TaskItemMapper.ToDto(t));
        }

        public async Task<IEnumerable<TaskItemDto>> GetAllTaskItemsByFilter(FilterTaskItemDto dto) {
            TaskItemFilterQueryModel filterQuery = TaskItemMapper.ToFilterQueryModel(dto);
            IEnumerable<TaskItem> taskItems = await _taskItemRepository.GetAllTaskItemsByFilter(filterQuery);
            return taskItems.Select(t => TaskItemMapper.ToDto(t));
        }

        public async Task<IEnumerable<Comment>> GetChangeLogs(Guid taskItemId) {
            
        }

        public async Task UpdateTaskItemAsync(UpdateTaskItemRequestDto dto)
        {
            TaskItem taskItem = (TaskItem)_taskItemRepository.GetByIdAsync(dto.Id) ?? throw new Exception($"TaskId {dto.Id} does not exist");

            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                taskItem.Title = dto.Title;
            }

            if (!string.IsNullOrEmpty(dto.Description))
            {
                taskItem.Description = dto.Description;
            }

            if (!string.IsNullOrWhiteSpace(dto.Type))
            {
                if (!Enum.TryParse<TaskItemType>(dto.Type, out TaskItemType type))
                {
                    throw new Exception("Type is invalid");
                }
                taskItem.Type = type;
            }

            if (!string.IsNullOrWhiteSpace(dto.Priority))
            {
                if (!Enum.TryParse<TaskItemPriority>(dto.Priority, out TaskItemPriority priority))
                {
                    throw new Exception("Priority is invalid");
                }
                taskItem.Priority = priority;
            }

            if (!string.IsNullOrWhiteSpace(dto.Status))
            {
                if (!Enum.TryParse<TaskItemStatus>(dto.Status, out TaskItemStatus status))
                {
                    throw new Exception("Status is invalid");
                }
                taskItem.Status = status;
            }

            if (dto.AssignedUserId != null)
            {
                taskItem.AssignedUser = await _userRepository.GetByIdAsync((Guid)dto.AssignedUserId) ?? throw new Exception($"UserId {dto.AssignedUserId} not found");
            }

            if (dto.Deadline != null)
            {
                taskItem.Deadline = dto.Deadline;
            }

            if (dto.Tags != null)
            {
                ICollection<Tag> tags = new List<Tag>();
                foreach (string name in dto.Tags)
                {
                    tags.Add(await _tagRepository.GetOrCreateAsync(name));
                }
                taskItem.Tags = tags;
            }

            await _taskItemRepository.UpdateAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();
        }

        public async Task DeleteTaskItemAsync(Guid taskItemId)
        {
            TaskItem taskItem = await _taskItemRepository.GetByIdAsync(taskItemId) ?? throw new Exception($"TaskId {taskItemId} not found");
            await _taskItemRepository.DeleteAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();
        }

       

  

        public async Task<ICollection<TaskItem>> GetAllTaskItemsByFiler(TaskItemFilterRequestDto dto)
        {
            
            if (Enum.TryParse<TaskItemType>(dto.Type, out TaskItemType? parsed)) {
          
            }

            TaskItemPriority? priority = null;
            if (Enum.TryParse<TaskItemPriority>(dto.Priority, out TaskItemPriority parsed))
            {
                priority = parsed;
            }

                TaskItemFilterQM queryModel = new TaskItemFilterQM
                {
                    ProjectId = dto.ProjectId,
                    AssignedUserId = dto.AssignedUserId,
                    Enum.TryParse<TaskItemType>(dto.Type, ignoreCase: true, out var a);

                };
            return await _taskItemRepository.GetAllTaskItemsByFilter();
        }

    }
}