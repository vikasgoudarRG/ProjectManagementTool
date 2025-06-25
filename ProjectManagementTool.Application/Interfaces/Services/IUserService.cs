using ProjectManagementTool.Application.DTOs.User;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserService
    {
        // Create
        Task<UserDTO> AddAsync(AddUserDTO user);

        // Read
        Task<UserDTO> GetByIdAsync(Guid userId);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<IEnumerable<UserDTO>> SearchAsync(string keyword);

        // Update
        Task UpdateAsync(Guid userId, UpdateUserDTO user);

        // Delete
        Task DeleteAsync(Guid userId);
    }
}