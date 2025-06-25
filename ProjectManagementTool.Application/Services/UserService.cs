using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Application.Mappers;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;

namespace ProjectManagementTool.Application.Services
{
    public class UserService : IUserService
    {
        // ======================= Fields ======================= //
        private readonly IUserRepository _userRepository;
        private readonly IUserMapper _userMapper;
        private readonly IUnitOfWork _unitOfWork;

        // ==================== Constructors ==================== //
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserMapper userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
            _unitOfWork = unitOfWork;
        }

        // ======================= Methods ====================== //
        // Create
        public async Task<UserDTO> AddAsync(AddUserDTO addUserDto)
        {
            var user = new User(addUserDto.Name, addUserDto.Email, addUserDto.Password);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return _userMapper.ToDTO(user);
        }

        // Read
        public async Task<UserDTO> GetByIdAsync(Guid userId)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            return user == null
                    ? throw new KeyNotFoundException("Id not found")
                    : _userMapper.ToDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            IEnumerable<User> users = await _userRepository.GetAllAsync();
            return users.Select(user => _userMapper.ToDTO(user));
        }

        public async Task<IEnumerable<UserDTO>> SearchAsync(string keyword)
        {
            IEnumerable<User> users = await _userRepository.SearchAsync(keyword);
            return users.Select(user => _userMapper.ToDTO(user));
        }

        // Update
        public async Task UpdateAsync(Guid userId, UpdateUserDTO updateUserDto)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrWhiteSpace(updateUserDto.Name))
                user.Name = updateUserDto.Name;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
                user.Email = updateUserDto.Email;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
                user.Password = updateUserDto.Password;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteAsync(Guid userId)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            await _userRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
