using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.DTOs.Notification;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Application.Mappers;
using System.Data.Common;
using ProjectManagementTool.Domain.Enums.Project;
using System.Linq.Expressions;

namespace ProjectManagementTool.Application.Services
{
    public class ProjectService : IProjectService
    {
        // ======================= Fields ======================= //
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectChangeLogRepository _changeLogRepository;
        private readonly IUserNotificationService _notificationService;
        private readonly IProjectMapper _projectMapper;
        private readonly IUserMapper _userMapper;
        private readonly IUnitOfWork _unitOfWork;

        // ==================== Constructors ==================== //
        public ProjectService(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IProjectChangeLogRepository changeLogRepository,
            IUserNotificationService notificationService,
            IProjectMapper projectMapper,
            IUserMapper userMapper,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _changeLogRepository = changeLogRepository;
            _notificationService = notificationService;
            _projectMapper = projectMapper;
            _userMapper = userMapper;
            _unitOfWork = unitOfWork;
        }

        // ======================= Methods ====================== //
        // Create
        public async Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO createProjectDto)
        {
            User user = await _userRepository.GetByIdAsync(createProjectDto.ProjectLeadId)
                ?? throw new KeyNotFoundException("Project lead not found");

            Project project = new Project(createProjectDto.Name, createProjectDto.Description, createProjectDto.ProjectLeadId);
            await _projectRepository.AddAsync(project);

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                project.Id, createProjectDto.ProjectLeadId, ChangeType.Created, "Project", null, project.Name));

            UserNotification userNotification = new UserNotification(
                userId: createProjectDto.ProjectLeadId,
                message: $"Created Project: {project.Id}"
            );
            await _notificationService.SendUserNotification(userNotification);

            await _unitOfWork.SaveChangesAsync();
            return _projectMapper.ToDTO(project);
        }

        // Get
        public async Task<ProjectDTO> GetByIdAsync(Guid projectId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new KeyNotFoundException("Project not found");
            return _projectMapper.ToDTO(project);
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllForUserAsync(Guid userId)
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllByUserIdAsync(userId);
            return projects.Select(p => _projectMapper.ToDTO(p));
        }

        public async Task<IEnumerable<UserDTO>> GetAllDevelopersAsync(Guid projectId, Guid requesterId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Not authorized to view project");

            return project.Developers.Select(d => _userMapper.ToDTO(d));
        }

        // Update
        public async Task AddDeveloperAsync(Guid requestorId, ProjectDeveloperDTO addDeveloperDto)
        {
            User user = await _userRepository.GetByIdAsync(addDeveloperDto.UserId)
                ?? throw new KeyNotFoundException("Developer not found");

            Project project = await _projectRepository.GetByIdAsync(addDeveloperDto.ProjectId)
                ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != requestorId)
                throw new UnauthorizedAccessException("Not authorized to add developer");

            project.AddDeveloper(user);
            await _projectRepository.UpdateAsync(project);

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                requestorId, addDeveloperDto.ProjectId, ChangeType.Created, "Developer", null, addDeveloperDto.UserId.ToString()));

            UserNotification userNotification = new UserNotification(
                userId: addDeveloperDto.UserId,
                message: $"Added to project: {project.Id}"
            );
            await _notificationService.SendUserNotification(userNotification);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveDeveloperAsync(Guid requestorId, ProjectDeveloperDTO removeDeveloperDto)
        {
            User user = await _userRepository.GetByIdAsync(removeDeveloperDto.UserId)
                ?? throw new KeyNotFoundException("Developer not found");

            Project project = await _projectRepository.GetByIdAsync(removeDeveloperDto.ProjectId)
                ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != requestorId)
                throw new UnauthorizedAccessException("Not Authorized to remove developer");

            project.RemoveDeveloper(user);
            await _projectRepository.UpdateAsync(project);

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                removeDeveloperDto.ProjectId, requestorId, ChangeType.Deleted, "Developer", removeDeveloperDto.UserId.ToString(), null));

            UserNotification userNotification = new UserNotification(
                userId: removeDeveloperDto.UserId,
                message: $"Remove from project: {removeDeveloperDto.ProjectId}"
            );
            await _notificationService.SendUserNotification(userNotification);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid requestorId, Guid projectId, UpdateProjectDTO updateProjectDto)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != requestorId)
                throw new UnauthorizedAccessException("Not authorized to delete project");

            if (updateProjectDto.Name != null)
                project.Name = updateProjectDto.Name;

            if (updateProjectDto.Description != null)
                project.Description = updateProjectDto.Description;

            if (updateProjectDto.Status != null)
            {
                if (Enum.TryParse<ProjectStatus>(updateProjectDto.Status, ignoreCase: true, out ProjectStatus status))
                {
                    project.Status = status;
                }
                else
                {
                    throw new ArgumentException("Project status not valid");
                }
            }
            await _projectRepository.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteProjectAsync(Guid projectId, Guid requesterId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new KeyNotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Not authorized to delete project");

            await _projectRepository.DeleteAsync(project);

            await _changeLogRepository.AddAsync(new ProjectChangeLog(
                projectId, requesterId, ChangeType.Deleted, "Project", project.Name, null));

            UserNotification userNotification = new UserNotification(
                userId: requesterId,
                message: $"Deleted Project: {projectId}"
                );
            await _notificationService.SendUserNotification(userNotification);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
