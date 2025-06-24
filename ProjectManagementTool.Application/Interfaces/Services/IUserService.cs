using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task AddAsync(CreateUserDTO user);
        Task<UserDTO?> GetByIdAsync(Guid userId);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<IEnumerable<UserDTO>> SearchAsync(string keyword);
        Task UpdateAsync(UpdateUserDTO user);
        Task DeleteAsync(Guid userId);
    }
}