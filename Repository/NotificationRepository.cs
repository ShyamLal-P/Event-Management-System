using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using EventManagementWithAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly EventManagementSystemDbContext _context;

        public NotificationRepository(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task AddNotificationAsync(Notification notificationItem)
        {
            notificationItem.Id = Guid.NewGuid();
            _context.Notifications.Add(notificationItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(Notification notificationItem)
        {
            _context.Entry(notificationItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Guid id)
        {
            var notificationItem = await _context.Notifications.FindAsync(id);
            if (notificationItem != null)
            {
                _context.Notifications.Remove(notificationItem);
                await _context.SaveChangesAsync();
            }
        }

        public bool NotificationExists(Guid id)
        {
            return _context.Notifications.Any(n => n.Id == id);
        }
    }
}
