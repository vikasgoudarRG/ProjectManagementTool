using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;

namespace ProjectManagementTool.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAsync(CreateUserDTO dto)
        {
            var user = new User(dto.Name, dto.Email, dto.Password);
            await _userRepository.AddAsync(user);
        }

        public async Task<UserDTO?> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : new UserDTO
            {
                Id = user.Id,
                Name = user.Username,
                Email = user.Email
            };
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Username,
                Email = user.Email
            });
        }

        public async Task<IEnumerable<UserDTO>> SearchAsync(string keyword)
        {
            var users = await _userRepository.SearchAsync(keyword);
            return users.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Username,
                Email = user.Email
            });
        }

        public async Task UpdateAsync(UpdateUserDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id);
            if (user == null) return;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.UpdateName(dto.Name);

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.UpdateEmail(dto.Email);

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.UpdatePassword(dto.Password);

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllInProjectAsync(Guid projectId)
        {
            var users = await _userRepository.GetAllAsync(); // You can refactor this to query more efficiently
            return users
                .Where(u => u.Projects.Any(p => p.ProjectId == projectId))
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Username,
                    Email = user.Email
                });
        }

        public async Task<IEnumerable<UserDTO>> GetAllInTeamAsync(Guid teamId)
        {
            var users = await _userRepository.GetAllAsync(); // You can refactor this to query more efficiently
            return users
                .Where(u => u.TeamMemberships.Any(t => t.TeamId == teamId))
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Username,
                    Email = user.Email
                });
        }

        public async Task<bool> IsUserInProjectAsync(Guid userId, Guid projectId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.Projects.Any(p => p.ProjectId == projectId) ?? false;
        }

        public async Task<bool> IsUserInTeamAsync(Guid userId, Guid teamId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.TeamMemberships.Any(t => t.TeamId == teamId) ?? false;
        }
    }
}
