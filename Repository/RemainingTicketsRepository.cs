using EventManagementSystem.Data;
using EventManagementSystem.Interface;

namespace EventManagementSystem.Repository
{
    public class RemainingTicketsRepository : IRemainingTicketsService
    {
        private readonly EventManagementSystemDbContext _context;

        public RemainingTicketsRepository(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetRemainingTicketsAsync(Guid eventId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null)
            {
                throw new KeyNotFoundException("Event not found");
            }
            return eventItem.TotalTickets - eventItem.NoOfTickets;
        }
    }
}
