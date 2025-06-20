using System.Data.Common;
using ProjectManagementTool.Application.DTOs.TaskItemChangeLog;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public static class TaskItemChangeLogMapper
    {
        public static TaskItemChangeLogDto ToDto(TaskItemChangeLog changeLog)
        {
            return new TaskItemChangeLogDto
            {
                Id = changeLog.Id,
                TaskItemId = changeLog.TaskItemId,
                ChangedByUserDto = UserMapper.ToDto(changeLog.ChangedByUser),
                PropertyChanged = changeLog.PropertyChanged,
                OldValue = changeLog.OldValue,
                NewValue = changeLog.NewValue,
                CreatedOn = changeLog.CreatedOn
            };
        }

        public static IEnumerable<TaskItemChangeLogDto> ToDtoRange(IEnumerable<TaskItemChangeLog> changeLogs)
        {
            return changeLogs.Select(l => ToDto(l)).ToList();
        }
    }
}