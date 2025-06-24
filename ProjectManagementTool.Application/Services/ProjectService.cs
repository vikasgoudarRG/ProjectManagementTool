using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Entities.ChangeLogs;
using ProjectManagementTool.Domain.Enums.ChangeLog;
using ProjectManagementTool.Domain.Exceptions;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;

namespace ProjectManagementTool.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectChangeLogRepository _changeLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IProjectChangeLogRepository changeLogRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _changeLogRepository = changeLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateProjectAsync(CreateProjectDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.ProjectLeadId)
                        ?? throw new NotFoundException("User not found");

            var project = new Project(dto.Name, dto.Description, dto.ProjectLeadId);

            await _projectRepository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            var log = new ProjectChangeLog(project.Id, "Project", null, project.Name, ChangeType.Created, dto.ProjectLeadId);
            await _changeLogRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            return project.Id;
        }

        public async Task<ProjectDTO?> GetByIdAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null) return null;

            return new ProjectDTO
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
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only the project lead can delete the project.");

            await _projectRepository.DeleteAsync(project);

            var log = new ProjectChangeLog(projectId, "Project", project.Name, null, ChangeType.Deleted, requesterId);
            await _changeLogRepository.AddAsync(log);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddDeveloperAsync(ProjectUserActionDTO dto)
        {
            var project = await _projectRepository.GetByIdAsync(dto.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only the project lead can add developers.");

            await project.AddDeveloperAsync(dto.DeveloperId);

            await _projectRepository.UpdateAsync(project);

            var log = new ProjectChangeLog(dto.ProjectId, "DeveloperIds", null, dto.DeveloperId.ToString(), ChangeType.Added, dto.RequesterId);
            await _changeLogRepository.AddAsync(log);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveDeveloperAsync(ProjectUserActionDTO dto)
        {
            var project = await _projectRepository.GetByIdAsync(dto.ProjectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != dto.RequesterId)
                throw new UnauthorizedAccessException("Only the project lead can remove developers.");

            await project.RemoveDeveloperAsync(dto.DeveloperId);
            await _projectRepository.UpdateAsync(project);

            var log = new ProjectChangeLog(dto.ProjectId, "DeveloperIds", dto.DeveloperId.ToString(), null, ChangeType.Removed, dto.RequesterId);
            await _changeLogRepository.AddAsync(log);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllDevelopersAsync(Guid projectId, Guid requesterId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                          ?? throw new NotFoundException("Project not found");

            if (project.ProjectLeadId != requesterId)
                throw new UnauthorizedAccessException("Only the project lead can view developers.");

            var userIds = project.DeveloperIds;
            var users = new List<UserDTO>();

            foreach (var id in userIds)
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user != null)
                {
                    users.Add(new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email
                    });
                }
            }

            return users;
        }
    }
}
