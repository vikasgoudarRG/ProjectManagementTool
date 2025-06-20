using ProjectManagementTool.Application.DTOs.ProjectChangeLog;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Mappers
{
    public static class ProjectChangeLogMapper
    {
        public static ProjectChangeLogDto ToDto(ProjectChangeLog changeLog)
        {
            return new ProjectChangeLogDto
            {
                Id = changeLog.Id,
                ProjectId = changeLog.ProjectId,
                ChangedByUserDto = UserMapper.ToDto(changeLog.ChangedByUser),
                ChangeType = changeLog.ChangeType,
                PropertyChanged = changeLog.PropertyChanged,
                OldValue = changeLog.OldValue,
                NewValue = changeLog.NewValue,
                CreatedOn = changeLog.CreatedOn
            };
        }

        public static IEnumerable<ProjectChangeLogDto> ToDtoRange(IEnumerable<ProjectChangeLog> changeLogs)
        {
            return changeLogs.Select(l => ToDto(l)).ToList();
        }
    }
}