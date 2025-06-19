using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Repositories;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> CreateUserAsync(CreateUserRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                throw new Exception("Username is invalid");
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new Exception("Email is invalid");
            }

            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateUserAsync(UpdateUserRequestDto dto)
        {
            User user = await _userRepository.GetByIdAsync(dto.Id) ?? throw new Exception($"UserId {dto.Id} not found");
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

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            User user = await _userRepository.GetByIdAsync(userId) ?? throw new Exception($"UserId {userId} not found");
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}