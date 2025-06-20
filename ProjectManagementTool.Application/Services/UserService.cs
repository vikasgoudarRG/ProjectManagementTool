using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Mappers;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        public UserService(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            User user = new User(dto.Username, dto.Email);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return UserMapper.ToDto(user);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            User user = await _userRepository.GetByIdAsync(userId) ?? throw new Exception($"UserId {userId} not found");
            return UserMapper.ToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersForManagerAsync(Guid managerId)
        {
            IEnumerable<Project> allProjects = await _projectRepository.GetAllAsync();
            IEnumerable<User> users = allProjects.Where(p => p.ManagerId == managerId)
                                                 .SelectMany(p => p.Developers.Append(p.Manager))
                                                 .DistinctBy(u => u.Id);
            return users.Select(u => UserMapper.ToDto(u));
        }

        public async Task<IEnumerable<UserDto>> GetUsersInProjectAsync(Guid projectId)
        {
            Project project = await _projectRepository.GetByIdAsync(projectId) ?? throw new Exception($"ProjectId {projectId} not found");
            IEnumerable<User> users = project.Developers.Append(project.Manager).DistinctBy(u => u.Id);

            return users.Select(u => UserMapper.ToDto(u));
        }

        public async Task UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            User user = await _userRepository.GetByIdAsync(userId) ?? throw new Exception($"UserId {userId} not found");
            if (!string.IsNullOrWhiteSpace(dto.Username))
            {
                user.Username = dto.Username;
            }
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                user.Email = dto.Email;
            }

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            User user = await _userRepository.GetByIdAsync(userId) ?? throw new Exception($"UserId {userId} not found");

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
        }

    }
}