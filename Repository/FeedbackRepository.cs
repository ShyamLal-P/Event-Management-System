using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly EventManagementSystemDbContext _context;

        public FeedbackRepository(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(Guid id)
        {
            return await _context.Feedbacks.FindAsync(id);
        }

        public async Task AddFeedbackAsync(Feedback feedbackItem)
        {
            feedbackItem.Id = Guid.NewGuid();
            _context.Feedbacks.Add(feedbackItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedbackAsync(Feedback feedbackItem)
        {
            _context.Entry(feedbackItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedbackAsync(Guid id)
        {
            var feedbackItem = await _context.Feedbacks.FindAsync(id);
            if (feedbackItem != null)
            {
                _context.Feedbacks.Remove(feedbackItem);
                await _context.SaveChangesAsync();
            }
        }

        public bool FeedbackExists(Guid id)
        {
            return _context.Feedbacks.Any(f => f.Id == id);
        }
    }
}
