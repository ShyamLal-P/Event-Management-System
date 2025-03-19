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
    public class EventsController : ControllerBase
    {
        private readonly EventManagementSystemDbContext _context;

        public EventsController(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(Guid id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return eventItem;
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event eventItem)
        {
            eventItem.Id = Guid.NewGuid();
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] Event eventItem)
        {
            if (id != eventItem.Id)
            {
                return BadRequest("Event ID mismatch.");
            }

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.Name = eventItem.Name;
            existingEvent.Category = eventItem.Category;
            existingEvent.Location = eventItem.Location;
            existingEvent.Date = eventItem.Date;
            existingEvent.Time = eventItem.Time;
            existingEvent.OrganizerId = eventItem.OrganizerId;
            existingEvent.NoOfTickets = eventItem.NoOfTickets;
            existingEvent.EventPrice = eventItem.EventPrice;

            _context.Entry(existingEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}