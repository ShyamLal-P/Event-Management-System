using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly EventManagementSystemDbContext _context;

        public NotificationService(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetEventNotificationAsync(Guid eventId, string userId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .Where(t => t.EventId == eventId && t.UserId == userId)
                .ToListAsync();

            if (!tickets.Any())
            {
                return "No ticket found for the given event and user.";
            }

            var allCancelled = tickets.All(t => t.Status == "Cancelled");
            if (allCancelled)
            {
                return "No ticket found for the given event and user.";
            }

            var ticket = tickets.First(t => t.Status != "Cancelled");
            var eventDateTime = ticket.Event.Date.ToDateTime(ticket.Event.Time);
            var currentDateTime = DateTime.Now;

            if (currentDateTime < eventDateTime)
            {
                var timeLeft = eventDateTime - currentDateTime;
                return FormatTimeSpan(timeLeft);
            }
            else if (currentDateTime >= eventDateTime && currentDateTime <= eventDateTime.AddHours(2))
            {
                var timeAgo = currentDateTime - eventDateTime;
                return $"Event started {FormatTimeSpan(timeAgo)} ago.";
            }
            else
            {
                return "Event finished.";
            }
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            int months = (int)(timeSpan.TotalDays / 30);
            int days = timeSpan.Days % 30;
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;

            if (months > 0)
            {
                return $"{months} months {days} days {hours} hours {minutes} minutes remaining";
            }
            else if (days > 0)
            {
                return $"{days} days {hours} hours {minutes} minutes remaining";
            }
            else if (hours > 0)
            {
                return $"{hours} hours {minutes} minutes remaining";
            }
            else if (minutes > 0)
            {
                return $"{minutes} minutes remaining";
            }
            else
            {
                return "Event is going to start in 1 minute";
            }
        }
    }
}
