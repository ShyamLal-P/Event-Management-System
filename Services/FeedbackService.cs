using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly EventManagementSystemDbContext _context;

        public FeedbackService(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserHasBookedTicketAsync(Guid eventId, string userId)
        {
            return await _context.Tickets
                .AnyAsync(t => t.EventId == eventId && t.UserId == userId && t.Status == "booked");
        }

        public async Task<bool> IsEventStartedAsync(Guid eventId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null)
            {
                throw new InvalidOperationException("Event not found.");
            }

            return DateTime.Now >= eventItem.Date.ToDateTime(eventItem.Time);
        }

        public async Task<string> GetTimeUntilEventStartsAsync(Guid eventId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null)
            {
                throw new InvalidOperationException("Event not found.");
            }

            var timeRemaining = eventItem.Date.ToDateTime(eventItem.Time) - DateTime.Now;

            if (timeRemaining.TotalMinutes <= 0)
            {
                return "Feedback will be enabled soon.";
            }

            var months = (int)(timeRemaining.TotalDays / 30);
            var days = (int)(timeRemaining.TotalDays % 30);
            var hours = timeRemaining.Hours;
            var minutes = timeRemaining.Minutes;

            if (months > 0)
            {
                return $"{months} months {days} days {hours} hours {minutes} minutes";
            }
            if (days > 0)
            {
                return $"{days} days {hours} hours {minutes} minutes";
            }
            if (hours > 0)
            {
                return $"{hours} hours {minutes} minutes";
            }
            return $"{minutes} minutes";
        }

        public async Task SubmitFeedbackAsync(Guid eventId, string userId, Guid ticketId, int rating, string comments)
        {
            // Check if the ticket exists and is booked
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == ticketId && t.Status == "booked");

            if (ticket == null)
            {
                throw new InvalidOperationException("Ticket not found or not booked.");
            }

            // Check if feedback already exists for the ticket
            var existingFeedback = await _context.Feedbacks
                .AnyAsync(f => f.TicketId == ticketId);

            if (existingFeedback)
            {
                throw new InvalidOperationException("Feedback already submitted for this ticket.");
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EventId = eventId,
                Rating = rating,
                Comments = comments,
                SubmittedTime = TimeOnly.FromDateTime(DateTime.Now),
                SubmittedDate = DateOnly.FromDateTime(DateTime.Now),
                TicketId = ticketId
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> FeedbackExistsForTicketAsync(Guid ticketId)
        {
            return await _context.Feedbacks
                .AnyAsync(f => f.TicketId == ticketId);
        }
    }
}