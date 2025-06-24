using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Domain.Interfaces.Repositories
{
    public interface IUserNotificationRepository
    {
        Task AddAsync(UserNotification notification);

        Task<UserNotification?> GetByIdAsync(Guid notificationId);
        Task<IEnumerable<UserNotification>> GetAllByUserIdAsync(Guid userId);

        Task MarkAsReadAsync(Guid notificationId);


        Task DeleteAsync(UserNotification notifications);
        Task DeleteAllReadByUserIdAsync(Guid userId);
    }
}