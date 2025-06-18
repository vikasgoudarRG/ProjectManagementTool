using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(CreateUserRequestDto dto);
        Task UpdateUserAsync(UpdateUserRequestDto dto);
        Task DeleteUserAsync(Guid userId);
        Task<UserDto> GetUserByIdAsync(Guid userId);
    }

}