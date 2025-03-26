using EventManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemainingTicketsController : ControllerBase
    {
        private readonly IRemainingTicketsService _remainingTicketsService;

        public RemainingTicketsController(IRemainingTicketsService remainingTicketsService)
        {
            _remainingTicketsService = remainingTicketsService;
        }

        // GET: api/RemainingTickets/{eventId}
        [Authorize(Roles = "Admin, User")]
        [HttpGet("{eventId}")]
        public async Task<ActionResult<int>> GetAvailableTickets(Guid eventId)
        {
            try
            {
                var availableTickets = await _remainingTicketsService.GetRemainingTicketsAsync(eventId);
                return Ok(availableTickets);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Event not found");
            }
        }
    }
}
