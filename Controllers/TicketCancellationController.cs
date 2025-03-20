using EventManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCancellationController : ControllerBase
    {
        private readonly ITicketCancellationService _ticketCancellationService;

        public TicketCancellationController(ITicketCancellationService ticketCancellationService)
        {
            _ticketCancellationService = ticketCancellationService;
        }

        // POST: api/TicketCancellation
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> CancelTickets(Guid userId, Guid eventId, int numberOfTickets)
        {
            var (success, message) = await _ticketCancellationService.CancelTicketsAsync(userId, eventId, numberOfTickets);
            if (success)
            {
                var refundAmount = await _ticketCancellationService.CalculateRefundAmountAsync(eventId, numberOfTickets);
                return Ok(new { message, refundAmount });
            }
            return BadRequest(new { message });
        }
    }
}
