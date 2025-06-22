using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(UserNotification notification);

        Task<UserNotification?> GetByIdAsync(Guid notificationId);
        Task<IEnumerable<UserNotification>> GetAllByUserIdAsync(Guid userId);

        Task UpdateAsync(UserNotification notifications);

        Task DeleteAsync(UserNotification notifications);
    }
}