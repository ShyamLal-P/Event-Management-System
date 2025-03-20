using EventManagementSystem.Interface;
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
        [HttpGet("{eventId}")]
        public async Task<ActionResult<int>> GetNoOfTickets(Guid eventId)
        {
            try
            {
                var noOfTickets = await _remainingTicketsService.GetRemainingTicketsAsync(eventId);
                return Ok(noOfTickets);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Event not found");
            }
        }
    }
}
