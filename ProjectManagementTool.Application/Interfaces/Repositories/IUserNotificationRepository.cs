using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface IUserNotificationRepository
    {
        Task AddAsync(UserNotification notification);

        Task<UserNotification?> GetByIdAsync(Guid notificationId);
        Task<IEnumerable<UserNotification>> GetAllByUserIdAsync(Guid userId);

        Task DeleteAsync(UserNotification notifications);
    }
}