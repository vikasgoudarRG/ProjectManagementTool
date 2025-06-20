using System.ComponentModel.Design;
using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Mappers;
using ProjectManagementTool.Application.QueryModels;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserService _userService;
        public ProjectService(IProjectRepository projectRepository, IUserService userService)
        {
            _projectRepository = projectRepository;
            _userService = userService;
        }
        public async Task<ProjectDto> CreateProjectAsync(Guid creatorUserId, CreateProjectDto dto)
        {
            UserDto manager = await _userService.GetUserByIdAsync(creatorUserId) ?? throw new Exception($"ManagerId {creatorUserId} not found");

            ICollection<User> developers = new List<User>();
            if (dto.DeveloperIds != null && dto.DeveloperIds.Any())
            {
                foreach (Guid devId in dto.DeveloperIds)
                {
                    User developer = await _userRepository.GetByIdAsync(devId) ?? throw new Exception($"DeveloperId {devId} not found");
                }
            }


            Project project = new Project(dto.Title, dto.Description, creatorUserId, developers);

            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            return ProjectMapper.ToDto(project);
        }

        public async Task<ProjectDto> GetProjectByIdAsync(Guid projectId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectTd {projectId} not found");
            return ProjectMapper.ToDto(project);

        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsForUserAsync(Guid userId)
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllByUserIdAsync(userId) ?? throw new Exception($"User {userId} not found");

            return projects.Select(p => ProjectMapper.ToDto(p));
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsByFilterAsync(FilterProjectDto queryModel)
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllByFilterAsync(ProjectMapper.ToFilterQueryModel(queryModel));
            return projects.Select(p => ProjectMapper.ToDto(p));
        }

        public async Task<ProjectSummaryDto> GetProjectSummaryAsync(Guid projectId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"Project {projectId} not found");

            return ProjectMapper.ToSummaryDto(project);
        }

        public async Task AssignDevelopersAsync(Guid projectId, IEnumerable<Guid> developerIds)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectId {projectId} not found");
            foreach (Guid devId in developerIds)
            {
                User developer = await _userRepository.GetByIdAsync(devId) ?? throw new Exception($"UserId {devId} not found");
                project.Developers.Add(developer);
            }
            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Guid projectId, UpdateProjectDto dto)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectId {projectId} not found");

            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                project.Title = dto.Title;
            }
            if (!string.IsNullOrEmpty(dto.Description))
            {
                project.Description = dto.Description;
            }

            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task MarkProjectStatusAsync(Guid projectId, string status)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectId {projectId} not found");
            project.Status = Enum.TryParse<ProjectStatus>(status, out ProjectStatus status_out) ? status_out : throw new Exception($"Status {status} is invlalid");
        }

        public async Task RemoveDevelopersAsync(Guid projectId, IEnumerable<Guid> developerIds)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectId {projectId} not found");
            foreach (Guid devId in developerIds)
            {
                User dev = await _userRepository.GetByIdAsync(devId) ?? throw new Exception($"UserId {devId} not found");
                project.Developers.Remove(dev);
            }
            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectId {projectId} not found");

            await _projectRepository.DeleteAsync(project);
            await _projectRepository.SaveChangesAsync();
        }
    }
}