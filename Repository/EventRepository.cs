using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using EventManagementWithAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EventManagementSystemDbContext _context;

        public EventRepository(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(Guid id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task AddEventAsync(Event eventItem)
        {
            eventItem.Id = Guid.NewGuid();
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event eventItem)
        {
            _context.Entry(eventItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Guid id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }
        }

        public bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        public void SetTotalTickets(Event eventEntity, int totalTickets)
        {
            eventEntity.SetTotalTickets(totalTickets);
        }
    }
}

