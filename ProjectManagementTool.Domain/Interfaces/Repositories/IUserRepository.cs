using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // Create
        Task AddAsync(User user);

        // Read
        Task<User?> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync(); 
        Task<IEnumerable<User>> SearchAsync(string keyword); 

        // Update
        Task UpdateAsync(User user);

        // Delete
        Task DeleteAsync(User user);
    }

}