using System.ComponentModel;
using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Application.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDto ToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status.ToString(),
                CreatedOn = project.CreatedOn,
                ManagerId = project.ManagerId,
                ManagerUsername = project.Manager.Username,
                Developers = project.Developers.Select(u => UserMapper.ToSimpleDto(u))
            };
        }

        public static ProjectSummaryDto ToSummaryDto(Project project)
        {
            int totalTasks = project.TaskItems.Count;

            IDictionary<string, int> taskCountByType = project.TaskItems
                .GroupBy(t => t.Type.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            IDictionary<string, int> taskCountByStatus = project.TaskItems
                .GroupBy(t => t.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            return new ProjectSummaryDto
            {
                Id = project.Id,
                Title = project.Title,
                Status = project.Status.ToString(),
                CreatedOn = project.CreatedOn,
                ManagerId = project.ManagerId,
                ManagerUsername = project.Manager.Username,

                TotalTaskItems = totalTasks,
                TaskItemCountByType = taskCountByType,
                TaskItemCountByStatus = taskCountByStatus,
                Developers = project.Developers.Select(d => UserMapper.ToSimpleDto(d))
            };
        }

        public static ProjectFilterQueryModel ToFilterQueryModel(FilterProjectDto dto)
        {
            return new ProjectFilterQueryModel
            {
                TitleKeyword = dto.TitleKeyword,
                ManagerId = dto.ManagerId,
                Status = Enum.TryParse<ProjectStatus>(dto.Status, ignoreCase: true, out ProjectStatus status) ? status : throw new Exception($"Status {dto.Status} is invalid"),
                DeveloperIds = dto.DeveloperIds,
                CreatedBefore = dto.CreatedBefore,
                CreatedAfter = dto.CreatedAfter
            };
        }
    }
}