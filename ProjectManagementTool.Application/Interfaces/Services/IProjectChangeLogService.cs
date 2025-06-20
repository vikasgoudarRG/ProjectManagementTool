using ProjectManagementTool.Application.DTOs.ProjectChangeLog;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IProjectChangeLogService
    {
        Task CreateLogChangeAsync(CreateProjectChangeLogDto dto);
        Task<ProjectChangeLogDto> GetChangeLogById(Guid id);
        Task<IEnumerable<ProjectChangeLogDto>> GetChangeLogsByProjectIdAsync(Guid projectId);
    }
}