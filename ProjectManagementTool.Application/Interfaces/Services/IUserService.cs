using ProjectManagementTool.Application.DTOs.User;
using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Services
{
    public interface IUserService
    {
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    Task<IEnumerable<User>> GetAllInProjectAsync(Guid projectId);
    Task<IEnumerable<User>> GetAllInTeamAsync(Guid teamId);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> SearchAsync(string keyword);

    Task<bool> IsUserInProjectAsync(Guid userId, Guid projectId);
    Task<bool> IsUserInTeamAsync(Guid userId, Guid teamId);

    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    }

}