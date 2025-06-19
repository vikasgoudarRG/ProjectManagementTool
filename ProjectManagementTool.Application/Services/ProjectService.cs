using ProjectManagementTool.Application.DTOs.Project;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Enums.Project;

namespace ProjectManagementTool.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }
        public async Task<Guid> CreateProjectAsync(CreateProjectRequestDto dto)
        {
            User manager = await _userRepository.GetByIdAsync(dto.ManagerId) ?? throw new Exception($"ManagerId {dto.ManagerId} not found");

            ICollection<User> developers = new List<User>();

            dto.DeveloperIds = dto.DeveloperIds
                .Append(dto.ManagerId)
                .Distinct()
                .ToList();

            foreach (Guid devId in dto.DeveloperIds)
            {
                User? developer = await _userRepository.GetByIdAsync(devId);
                if (developer != null)
                {
                    developers.Add(developer);
                }
                else
                {
                    throw new Exception($"DeveloperId {devId} not found");
                }
            }

            Project project = new Project(dto.Title, dto.Description, dto.ManagerId)
            {
                ManagerId = dto.ManagerId,
                Developers = developers
            };
            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            return project.Id;
        }
        public async Task UpdateProjectAsync(UpdateProjectRequestDto dto)
        {
            Project? project = await _projectRepository.GetByIdAsync(dto.Id) ?? throw new Exception($"ProjectId {dto.Id} not found");

            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                project.Title = dto.Title;
            }
            if (!string.IsNullOrEmpty(dto.Description))
            {
                project.Description = dto.Description;
            }
            if (dto.ManagerId != null)
            {
                project.Manager = await _userRepository.GetByIdAsync((Guid)dto.ManagerId) ?? throw new Exception($"ManagerId {dto.ManagerId} not found");
            }
            if (!string.IsNullOrWhiteSpace(dto.Status))
            {

                if (!Enum.TryParse<ProjectStatus>(dto.Status, ignoreCase: true, out var status))
                {
                    throw new Exception($"Status {dto.Status} is invalid");
                }
                project.Status = status;

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
        public async Task AssignUsersToProjectAsync(AssignProjectDevelopers dto)
        {
            Project project = await _projectRepository.GetByIdAsync(dto.ProjectId) ?? throw new Exception($"ProjectId {dto.ProjectId} not found");
            foreach (Guid developerId in dto.DeveloperIds)
            {
                User developer = await _userRepository.GetByIdAsync(developerId) ?? throw new Exception($"UserId {developerId} not found");
                project.Developers.Add(developer);
            }
            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();
        }
        public async Task<ProjectDto> GetProjectByIdAsync(Guid projectId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectTd {projectId} not found");
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status.ToString(),
                ManagerName = project.Manager.Username,
                DeveloperUsernames = project.Developers.Select(u => u.Username).ToList()
            };
          
        }
        public async Task<ICollection<ProjectDto>> GetProjectsForUserAsync(Guid userId)
        {
            ICollection<Project> projects = await _projectRepository.GetByUserIdAsync(userId) ?? throw new Exception($"User {userId} not found");

            return projects.Select(
                p => new ProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ManagerName = p.Manager.Username,
                    Status = p.Status.ToString(),
                    DeveloperUsernames = p.Developers.Select(d => d.Username).ToList()
                }
            ).ToList();
        }
        public async Task<ICollection<ProjectDto>> GetAllProjectsAsync()
        {
            ICollection<Project> projects = await _projectRepository.GetAllAsync();

            return projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status.ToString(),
                ManagerName = project.Manager.Username,
                DeveloperUsernames = project.Developers.Select(d => d.Username).ToList()
            }).ToList();
        }

        public async Task<ProjectSummaryDto> GetProjectSummaryAsync(Guid projectId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"Project {projectId} not found");

            int totalTasks = project.TaskItems.Count;
            IDictionary<string,int> taskCountByType = project.TaskItems
                .GroupBy(t => t.Type.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            IDictionary<string,int> taskCountByStatus = project.TaskItems
                .GroupBy(t => t.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            return new ProjectSummaryDto
            {
                Id = project.Id,
                Title = project.Title,
                Status = project.Status.ToString(),
                TotalTaskItems = totalTasks,
                TaskItemCountByType = taskCountByType,
                TaskItemCountByStatus = taskCountByStatus,
                AssignedDeveloperUsernames = project.Developers.Select(d => d.Username).ToList()
            };
        }
    }
}