using ProjectManagementTool.Application.DTOs.Tag;
using ProjectManagementTool.Application.DTOs.TaskItem;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;
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
        private readonly ITaskItemChangeLogRepository _taskItemChangeLogRepository;
        public TaskItemService(ITaskItemRepository taskItemRepository, ITagRepository tagRepository, IUserRepository userRepository, ITaskItemChangeLogRepository taskItemChangeLogRepository)
        {
            _taskItemRepository = taskItemRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _taskItemChangeLogRepository = taskItemChangeLogRepository;
        }

        public async Task<Guid> CreateTaskItemAsync(CreateTaskItemRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new Exception("Title cannot be empty");
            }
            if (!Enum.TryParse<TaskItemType>(dto.Type, out TaskItemType taskItemType))
            {
                throw new Exception("Type is invalid");
            }
            if (!Enum.TryParse<TaskItemPriority>(dto.Priority, out TaskItemPriority priority))
            {
                throw new Exception("Priority is invalid");
            }

            await _tagRepository.AddManyAsync(dto.Tags);
            await _tagRepository.SaveChangesAsync();

            TaskItem taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Type = taskItemType,
                Priority = priority,
                Status = TaskItemStatus.NotStarted,
                ProjectId = dto.ProjectId,
                AssignedUserId = dto.AssignedUserId,
                CreatedAt = DateTime.UtcNow,
                Deadline = dto.Deadline,
                Tags = tags
            };
            
            await _taskItemRepository.AddAsync(taskItem);
            await _taskItemRepository.SaveChangesAsync();

            return taskItem.Id;
        }

        public async Task UpdateTaskItemAsync(UpdateTaskItemRequestDto dto)
        {
            TaskItem taskItem = await _taskItemRepository.GetByIdAsync(dto.Id) ?? throw new Exception($"TaskId {dto.Id} does not exist");

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
                await _tagRepository.SaveChangesAsync();
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

        public async Task<TaskItemDto?> GetTaskItemById(Guid taskItemId)
        {
            TaskItem taskItem = await _taskItemRepository.GetByIdAsync(taskItemId) ?? throw new Exception($"TaskId {taskItemId} not found");
            ICollection<TagDto> tags = new List<TagDto>();
            foreach (Tag tag in taskItem.Tags)
            {
                TagDto tagDto = new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name
                };
                tags.Add(tagDto);
            }

            return new TaskItemDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Type = taskItem.Type.ToString(),
                Priority = taskItem.Priority.ToString(),
                ProjectId = taskItem.ProjectId,
                ProjectTitle = taskItem.Project.Title,
                AssignedUser = taskItem.AssignedUserId,
                AssignedUsername = (taskItem.AssignedUser != null) ? taskItem.AssignedUser.Username : "",
                CreatedAt = taskItem.CreatedAt,
                Deadline = taskItem.Deadline,
                Tags = tags
            };
        }

        public async Task<ICollection<TaskItem>> GetAllTaskItemsByProjectId(Guid projectId)
        {
            return await _taskItemRepository.GetAllByProjectId(projectId) ?? throw new Exception($"TaskId {projectId} not found");
        }

        public async Task<ICollection<TaskItem>> GetAllTaskItemsByFiler(TaskItemFilterRequestDto dto)
        {
            ICollection<Tag>? tags = null;
            if (dto.Tags != null)
            {
                tags = new List<Tag>();
                foreach (string string_tag in dto.Tags)
                {
                    Tag new_tag = await _tagRepository.GetOrCreateAsync(string_tag);
                    tags.Add(new_tag);
                }
                await _tagRepository.SaveChangesAsync();
            }

            TaskItemFilterQM queryModel = new TaskItemFilterQM
            {
                ProjectId = dto.ProjectId,
                AssignedUserId = dto.AssignedUserId,
                Type = Enum.TryParse<TaskItemType>(dto.Type, ignoreCase: true, out TaskItemType type) ? type : null,
                Priority = Enum.TryParse<TaskItemPriority>(dto.Priority, ignoreCase: true, out TaskItemPriority priority) ? priority : null,
                Status = Enum.TryParse<TaskItemStatus>(dto.Status, ignoreCase: true, out TaskItemStatus status) ? status : null,
                Tags = tags,
                DeadlineBefore = dto.DeadlineBefore,
                DeadlineAfter = dto.DeadlineAfter
            };
            return await _taskItemRepository.GetAllTaskItemsByFilter(queryModel);
        }

        public async Task<ICollection<TaskItemChangeLogDto>> GetChangeLogForTaskItem(Guid taskItemId) {
            ICollection<TaskItemChangeLog> taskItemChangeLog = await _taskItemChangeLogRepository.GetByTaskItemId(taskItemId);
            ICollection<TaskItemChangeLogDto> taskItemChangeLogDtos = new List<TaskItemChangeLogDto>();
            foreach (TaskItemChangeLog log in taskItemChangeLog)
            {
                TaskItemChangeLogDto logDto = new TaskItemChangeLogDto
                {
                    TaskItemTitle = log.TaskItem.Title,
                    TaskItemId = log.TaskItemId,
                    ChangedByUsername = log.ChangedByUser.Username,
                    PropertyChanged = log.PropertyChanged,
                    OldValue = log.OldValue ?? "",
                    NewValue = log.NewValue ?? "",
                    ChangedAt = log.ChangedAt
                };

                taskItemChangeLogDtos.Add(logDto);
            }

            return taskItemChangeLogDtos;
        }
    }
}