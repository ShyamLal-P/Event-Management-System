using EventManagementSystem.Interface;
using EventManagementSystem.Models.Dtos;
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
        public async Task<IActionResult> CancelTickets([FromBody] CancelTicketsRequest request)
        {
            var (success, message) = await _ticketCancellationService.CancelTicketsAsync(request.UserId, request.EventId, request.NumberOfTickets);
            if (success)
            {
                var refundAmount = await _ticketCancellationService.CalculateRefundAmountAsync(request.EventId, request.NumberOfTickets);
                return Ok(new { message, refundAmount });
            }
            return BadRequest(new { message });
        }

        // OPTIONS: api/TicketCancellation
        [HttpOptions]
        public IActionResult Preflight()
        {
            return Ok();
        }
    }
}
