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

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IEventRepository _eventRepository;

        public TicketController(ITicketRepository ticketRepository, IEventRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
        }

        // GET: api/Ticket
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var tickets = await _ticketRepository.GetAllTicketsAsync();
            return Ok(tickets);
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(Guid id)
        {
            var ticketItem = await _ticketRepository.GetTicketByIdAsync(id);
            if (ticketItem == null)
            {
                return NotFound();
            }
            return Ok(ticketItem);
        }

        // GET: api/Ticket/user/{userId}
        [Authorize(Roles = "User, Admin")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsByUserId(string userId)
        {
            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(userId);
            return Ok(tickets);
        }

        // GET: api/Ticket/user/{userId}/events
        [Authorize(Roles = "User, Admin")]
        [HttpGet("user/{userId}/events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByUserId(string userId)
        {
            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(userId);
            if (tickets == null || !tickets.Any())
            {
                return NotFound("No tickets found for this user.");
            }

            var eventIds = tickets.Select(t => t.EventId).Distinct().ToList();
            var events = await _eventRepository.GetEventsByIdsAsync(eventIds);
            return Ok(events);
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            if (!_ticketRepository.TicketExists(id))
            {
                return NotFound();
            }

            await _ticketRepository.DeleteTicketAsync(id);
            return NoContent();
        }
    }
}
