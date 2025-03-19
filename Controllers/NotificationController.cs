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
    public class NotificationController : ControllerBase
    {
        private readonly EventManagementSystemDbContext _context;

        public NotificationController(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Notification
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        // GET: api/Notification/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(Guid id)
        {
            var notificationItem = await _context.Notifications.FindAsync(id);

            if (notificationItem == null)
            {
                return NotFound();
            }

            return notificationItem;
        }

        // POST: api/Notification
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notificationItem)
        {
            notificationItem.Id = Guid.NewGuid();
            _context.Notifications.Add(notificationItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotification), new { id = notificationItem.Id }, notificationItem);
        }

        // PUT: api/Notification/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(Guid id, [FromBody] Notification notificationItem)
        {
            if (id != notificationItem.Id)
            {
                return BadRequest("Notification ID mismatch.");
            }

            var existingNotification = await _context.Notifications.FindAsync(id);
            if (existingNotification == null)
            {
                return NotFound();
            }

            existingNotification.Message = notificationItem.Message;
            existingNotification.SentTime = notificationItem.SentTime;

            _context.Entry(existingNotification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
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

        private bool NotificationExists(Guid id)
        {
            return _context.Notifications.Any(n => n.Id == id);
        }

        // DELETE: api/Notification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var notificationItem = await _context.Notifications.FindAsync(id);
            if (notificationItem == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notificationItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}