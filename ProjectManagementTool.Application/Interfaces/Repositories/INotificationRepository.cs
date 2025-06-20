using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAllUnseenByUserIdAsync(Guid userId);
        Task UpdateAsync(Notification notifications);
        Task DeleteAsync(Notification notifications);
        Task SaveChangesAsync();
    }
}