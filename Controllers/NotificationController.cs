using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EventManagementSystem.Interface;
using EventManagementSystem.Services;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationRepository notificationRepository, INotificationService notificationService)
        {
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
        }

        // GET: api/Notification
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            var notifications = await _notificationRepository.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        [Authorize(Roles ="Admin, User")]
        [HttpGet("{eventId}/{userId}")]
        public async Task<IActionResult> GetEventNotification(Guid eventId, string userId)
        {
            var notification = await _notificationService.GetEventNotificationAsync(eventId, userId);
            return Ok(notification);
        }

        // GET: api/Notification/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(Guid id)
        {
            var notificationItem = await _notificationRepository.GetNotificationByIdAsync(id);
            if (notificationItem == null)
            {
                return NotFound();
            }
            return Ok(notificationItem);
        }

        // POST: api/Notification
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notificationItem)
        {
            await _notificationRepository.AddNotificationAsync(notificationItem);
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

            if (!_notificationRepository.NotificationExists(id))
            {
                return NotFound();
            }

            await _notificationRepository.UpdateNotificationAsync(notificationItem);
            return NoContent();
        }

        // DELETE: api/Notification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            if (!_notificationRepository.NotificationExists(id))
            {
                return NotFound();
            }

            await _notificationRepository.DeleteNotificationAsync(id);
            return NoContent();
        }
    }
}