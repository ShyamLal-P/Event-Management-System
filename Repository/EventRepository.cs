using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Event>> GetEventsByIdsAsync(List<Guid> eventIds)
        {
            return await _context.Events
                .Where(e => eventIds.Contains(e.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(string organizerId)
        {
            return await _context.Events.Where(e => e.OrganizerId == organizerId).ToListAsync();
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

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            return await _context.Events
                .Where(e => e.Date > today || (e.Date == today && e.Time > currentTime.ToTimeSpan()))
                .ToListAsync();
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
