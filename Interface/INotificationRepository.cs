using EventManagementSystem.Models;

namespace EventManagementSystem.Interface
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(Guid id);
        Task AddNotificationAsync(Notification notificationItem);
        Task UpdateNotificationAsync(Notification notificationItem);
        Task DeleteNotificationAsync(Guid id);
        bool NotificationExists(Guid id);
    }
}
