using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services
{
    public class AverageRatingService : IAverageRatingService
    {
        private readonly EventManagementSystemDbContext _context;

        public AverageRatingService(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<double> GetAverageRatingAsync(Guid eventId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.EventId == eventId)
                .ToListAsync();

            if (feedbacks.Count == 0)
            {
                return 0;
            }

            return feedbacks.Average(f => f.Rating);
        }
    }
}
