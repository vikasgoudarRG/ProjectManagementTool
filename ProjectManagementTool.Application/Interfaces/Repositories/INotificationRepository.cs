using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUnseenByUserIdAsync(Guid userId);
        Task UpdateAsync(IEnumerable<Notification> notifications);

        Task DeleteAsync(IEnumerable<Notification> notifications);

        Task SaveChangesAsync();
    }
}