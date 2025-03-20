using EventManagementSystem.Interface;
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
        [HttpPost]
        public async Task<IActionResult> CancelTickets(Guid userId, Guid eventId, int numberOfTickets)
        {
            var result = await _ticketCancellationService.CancelTicketsAsync(userId, eventId, numberOfTickets);
            if (result)
            {
                var refundAmount = await _ticketCancellationService.CalculateRefundAmountAsync(eventId, numberOfTickets);
                return Ok(new { message = "Tickets cancelled successfully.", refundAmount });
            }
            return BadRequest("Unable to cancel tickets.");
        }
    }
}
