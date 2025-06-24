using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task<User?> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync(); 
        Task<IEnumerable<User>> SearchAsync(string keyword); 

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);
    }

}