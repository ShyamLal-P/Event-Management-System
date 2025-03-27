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

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
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

        // POST: api/Ticket
        /*[HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticketItem)
        {
            await _ticketRepository.AddTicketAsync(ticketItem);
            return CreatedAtAction(nameof(GetTicket), new { id = ticketItem.Id }, ticketItem);
        }*/

        // PUT: api/Ticket/5
        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] Ticket ticketItem)
        {
            if (id != ticketItem.Id)
            {
                return BadRequest("Ticket ID mismatch.");
            }

            if (!_ticketRepository.TicketExists(id))
            {
                return NotFound();
            }

            await _ticketRepository.UpdateTicketAsync(ticketItem);
            return NoContent();
        }*/

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