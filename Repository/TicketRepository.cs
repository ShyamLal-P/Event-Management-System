﻿using EventManagementSystem.Data;
using EventManagementSystem.Interface;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly EventManagementSystemDbContext _context;

        public TicketRepository(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(Guid id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task AddTicketAsync(Ticket ticketItem)
        {
            ticketItem.Id = Guid.NewGuid();
            _context.Tickets.Add(ticketItem);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .Include(t => t.Event) // Include related Event data
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserAndEventAsync(Guid userId, Guid eventId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId.ToString() && t.EventId == eventId)
                .ToListAsync();
        }

        public async Task UpdateTicketAsync(Ticket ticketItem)
        {
            _context.Entry(ticketItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(Guid id)
        {
            var ticketItem = await _context.Tickets.FindAsync(id);
            if (ticketItem != null)
            {
                _context.Tickets.Remove(ticketItem);
                await _context.SaveChangesAsync();
            }
        }

        public bool TicketExists(Guid id)
        {
            return _context.Tickets.Any(t => t.Id == id);
        }
    }
}
