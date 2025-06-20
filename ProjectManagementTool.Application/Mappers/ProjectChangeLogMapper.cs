using ProjectManagementTool.Application.DTOs.ProjectChangeLog;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public static class ProjectChangeLogMapper
    {
        public static ProjectChangeLogDto ToDto(TaskItemChangeLog changeLog)
        {
            return new ProjectChangeLogDto
            {
                Id = changeLog.Id,
                ProjectId = changeLog.TaskItemId,
                ChangedByUserDto = UserMapper.ToDto(changeLog.ChangedByUser),
                PropertyChanged = changeLog.PropertyChanged,
                OldValue = changeLog.OldValue,
                NewValue = changeLog.NewValue,
                CreatedOn = changeLog.CreatedOn
            };
        }

        public static IEnumerable<ProjectChangeLogDto> ToDtoRange(IEnumerable<TaskItemChangeLog> changeLogs)
        {
            return changeLogs.Select(l => ToDto(l)).ToList();
        }
    }
}