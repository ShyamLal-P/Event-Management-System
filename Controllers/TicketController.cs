using EventManagementSystem.Data;
using EventManagementWithAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly EventManagementSystemDbContext _context;

        public TicketController(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Ticket
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(Guid id)
        {
            var ticketItem = await _context.Tickets.FindAsync(id);

            if (ticketItem == null)
            {
                return NotFound();
            }

            return ticketItem;
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticketItem)
        {
            ticketItem.Id = Guid.NewGuid();
            _context.Tickets.Add(ticketItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticketItem.Id }, ticketItem);
        }

        // PUT: api/Ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] Ticket ticketItem)
        {
            if (id != ticketItem.Id)
            {
                return BadRequest("Ticket ID mismatch.");
            }

            var existingTicket = await _context.Tickets.FindAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            existingTicket.EventId = ticketItem.EventId;
            existingTicket.UserId = ticketItem.UserId;
            existingTicket.BookingDate = ticketItem.BookingDate;
            existingTicket.Status = ticketItem.Status;

            _context.Entry(existingTicket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            var ticketItem = await _context.Tickets.FindAsync(id);
            if (ticketItem == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticketItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(Guid id)
        {
            return _context.Tickets.Any(t => t.Id == id);
        }
    }
}