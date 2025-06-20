using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersForManagerAsync(Guid managerId);
        Task<IEnumerable<UserDto>> GetUsersInProjectAsync(Guid projectId);
        Task UpdateUserAsync(Guid userId, UpdateUserDto dto);
        Task DeleteUserAsync(Guid userId);
    }

}