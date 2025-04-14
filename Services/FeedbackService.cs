using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using EventManagementSystem.Models;
using EventManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EventManagementSystem.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly EventManagementSystemDbContext _context;
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(EventManagementSystemDbContext context, IFeedbackRepository feedbackRepository)
        {
            _context = context;
            _feedbackRepository = feedbackRepository;
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

            var eventDateTime = eventItem.Date.ToDateTime(TimeOnly.MinValue).Add(eventItem.Time);
            return DateTime.Now >= eventDateTime;
        }

        public async Task<string> PostFeedbackAsync(string userId, Guid eventId, int rating, string comments)
        {
            // Check if the user has booked the event
            var hasBookedTicket = await UserHasBookedTicketAsync(eventId, userId);
            if (!hasBookedTicket)
            {
                return "User must book a ticket to submit feedback.";
            }

            // Optional: Check if feedback already exists for this event by this user
            var existingFeedback = await _feedbackRepository.GetAllFeedbacksAsync();
            if (existingFeedback.Any(f => f.EventId == eventId && f.UserId == userId))
            {
                return "Feedback already submitted for this event by the user.";
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EventId = eventId,
                Rating = rating,
                Comments = comments,
                SubmittedTime = TimeOnly.FromDateTime(DateTime.UtcNow),
                SubmittedDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _feedbackRepository.AddFeedbackAsync(feedback);
            return "Feedback posted successfully.";
        }

        public async Task<string> GetTimeUntilEventStartsAsync(Guid eventId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null)
            {
                throw new InvalidOperationException("Event not found.");
            }

            var eventDateTime = eventItem.Date.ToDateTime(TimeOnly.MinValue).Add(eventItem.Time);
            var timeRemaining = eventDateTime - DateTime.Now;

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

        public async Task SubmitFeedbackAsync(Guid eventId, string userId, int rating, string comments)
        {
            // Check if the user has a valid booked ticket for the event
            var hasBookedTicket = await _context.Tickets
                .AnyAsync(t => t.EventId == eventId && t.UserId == userId && t.Status == "booked");

            if (!hasBookedTicket)
            {
                throw new InvalidOperationException("User has not booked a ticket for this event.");
            }

            // Optional: Check if feedback already exists for this event by this user
            var existingFeedback = await _context.Feedbacks
                .AnyAsync(f => f.EventId == eventId && f.UserId == userId);

            if (existingFeedback)
            {
                throw new InvalidOperationException("Feedback already submitted for this event by the user.");
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EventId = eventId,
                Rating = rating,
                Comments = comments,
                SubmittedTime = TimeOnly.FromDateTime(DateTime.Now),
                SubmittedDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }
    }
}
