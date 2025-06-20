using System.Xml;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Services
{
    public class ProjectChangeLogService : IProjectChangeLogService
    {
        #region Fields
        private readonly IProjectChangeLogRepository _changeLogRepository;
        #endregion Fields

        #region Constructors
        public ProjectChangeLogService(IProjectChangeLogRepository changeLogRepository)
        {
            _changeLogRepository = changeLogRepository;
        }
        #endregion Constructors

        #region Methods
        public async Task CreateLogChangeAsync(CreateProjectChangeLogDto dto)
        {
            ProjectChangeLog changeLog = new ProjectChangeLog(
                projectId: dto.ProjectId,
                changedByUserId: dto.ChangedByUserId,
                changeType: dto.ChangeType,
                propertyChanged: dto.PropertyChanged,
                oldValue: dto.OldValue,
                newValue: dto.NewValue
            );

            await _changeLogRepository.AddAsync(changeLog);
        }

        public async Task<ProjectChangeLogDto> GetChangeLogById(Guid id)
        {
            ProjectChangeLog? changeLog = await _changeLogRepository.GetById(id);
            return changeLog != null ? ProjectChangeLog
        }
        Task<IEnumerable<ProjectChangeLogDto>> GetChangeLogsByProjectIdAsync(Guid projectId);
    }
}