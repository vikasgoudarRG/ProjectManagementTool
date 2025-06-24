using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;

namespace ProjectManagementTool.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectChangeLogRepository _changeLogRepository;
        private readonly IUserNotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IProjectChangeLogRepository changeLogRepository,
            IUserNotificationService notificationService,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _changeLogRepository = changeLogRepository;
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateProjectAsync(CreateProjectDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.ProjectLeadId)
                ?? throw new ArgumentException("Project lead not found");

            var project = new Project(dto.Name, dto.Description, dto.ProjectLeadId);
            await _projectRepository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                project.Id, dto.ProjectLeadId, ChangeType.Created, "Project", null, project.Name));

            await _notificationService.SendAsync(new SendNotificationDTO
            {
                UserId = dto.ProjectLeadId,
                Message = $"You created a new project: '{project.Name}'"
            });

            await _unitOfWork.SaveChangesAsync();
            return project.Id;
        }

        public async Task<ProjectDTO?> GetByIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            return project == null ? null : new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ProjectLeadId = project.ProjectLeadId,
                Status = project.Status.ToString(),
                CreatedOn = project.CreatedOn
            };
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllForUserAsync(Guid userId)
        {
            var projects = await _projectRepository.GetAllByUserIdAsync(userId);
            return projects.Select(p => new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ProjectLeadId = p.ProjectLeadId,
                Status = p.Status.ToString(),
                CreatedOn = p.CreatedOn
            });
        }

        public async Task DeleteProjectAsync(Guid projectId, Guid requesterId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only the project lead can delete the project.");

            await _projectRepository.DeleteAsync(project);

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                projectId, requesterId, ChangeType.Deleted, "Project", project.Name, null));

            await _notificationService.SendAsync(new SendNotificationDTO
            {
                UserId = requesterId,
                Message = $"You deleted the project: '{project.Name}'"
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddDeveloperAsync(ProjectUserActionDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.DeveloperId)
                ?? throw new ArgumentException("Developer not found");

            var project = await _projectRepository.GetByIdAsync(dto.ProjectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != dto.RequestingUserId)
                throw new UnauthorizedAccessException("Only the project lead can add developers.");

            project.AddDeveloper(user);
            await _projectRepository.UpdateAsync(project);

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                dto.ProjectId, dto.RequestingUserId, ChangeType.Created, "Developer", null, dto.DeveloperId.ToString()));

            await _notificationService.SendAsync(new SendNotificationDTO
            {
                UserId = dto.DeveloperId,
                Message = $"You have been added to project: '{project.Name}'"
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveDeveloperAsync(ProjectUserActionDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.DeveloperId)
                ?? throw new ArgumentException("Developer not found");

            var project = await _projectRepository.GetByIdAsync(dto.ProjectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != dto.RequestingUserId)
                throw new UnauthorizedAccessException("Only the project lead can remove developers.");

            project.RemoveDeveloper(user);
            await _projectRepository.UpdateAsync(project);

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                dto.ProjectId, dto.RequestingUserId, ChangeType.Deleted, "Developer", dto.DeveloperId.ToString(), null));

            await _notificationService.SendAsync(new SendNotificationDTO
            {
                UserId = dto.DeveloperId,
                Message = $"You have been removed from project: '{project.Name}'"
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllDevelopersAsync(Guid projectId, Guid requesterId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new ArgumentException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only the project lead can view project developers.");

            return project.Developers.Select(d => new UserDTO
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email
            });
        }
    }
}
