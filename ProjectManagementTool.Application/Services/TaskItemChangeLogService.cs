using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;

namespace ProjectManagementTool.Application.Services
{
    public class TaskItemChangeLogService : ITaskItemChangeLogService
    {
        #region Fields
        private readonly IChangeLogRepository _changeLogRepository;
        #endregion Fields

        #region Constructors
        public ChangeLogService(IChangeLogRepository changeLogRepository)
        {
            _changeLogRepository = changeLogRepository;
        }
        #endregion Constructors

        #region Methods
        #endregion Methods
    }
}