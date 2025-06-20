using System.Data.Common;
using ProjectManagementTool.Application.DTOs;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemChangeLogService : ITaskItemChangeLogService
    {
        #region Fields
        private readonly ITaskItemChangeLogRepository _taskItemChangeLogRepository;
        #endregion Fields

        #region Constructors
        public TaskItemChangeLogService(ITaskItemChangeLogRepository taskItemChangeLogRepository)
        {
            _taskItemChangeLogRepository = taskItemChangeLogRepository;
        }
        #endregion Constructors

        #region Methods
        public async Task CreateLogChangeAsync(CreateTaskItemChangeLogDto dto)
        {
            TaskItemChangeLog changeLog = new TaskItemChangeLog(
                taskItemId: dto.TaskItemId,
                changedByUserId: dto.ChangedByUserId,
                changeType: dto.ChangeType,
                propertyChanged: dto.PropertyChanged,
                oldValue: dto.OldValue,
                newValue: dto.NewValue
            );
            await _taskItemChangeLogRepository.AddAsync(changeLog);
        }

        public async Task<TaskItemChangeLogDto> GetChangeLogByIdAsync(Guid id)
        {
            TaskItemChangeLog? changeLog = await _taskItemChangeLogRepository.GetById(id);
            return changeLog != null ? TaskItemChangeLogMapper.ToDto(changeLog) : throw new Exception($"TaskItemChangeLogId {id} is invalid");
        }

        public async Task<IEnumerable<TaskItemChangeLogDto>> GetChangeLogsByTaskIdAsync(Guid taskId)
        {
            IEnumerable<TaskItemChangeLog> changeLogs = await _taskItemChangeLogRepository.GetAllByTaskItemId(taskId);
            return changeLogs.Select(l => TaskItemChangeLogMapper.ToDto(l));
        }
        #endregion Methods
    }
}