using ProjectManagementTool.Domain.Entities;

namespace ProjectManagementTool.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<ICollection<Notification>> GetUnseenByUserIdAsync(Guid userId);
        Task MarkAsSeenAsync(ICollection<Notification> notifications);

        Task SaveChangesAsync();
    }
}