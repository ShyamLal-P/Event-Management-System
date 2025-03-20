using EventManagementSystem.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalTicketsBookedController : ControllerBase
    {
        private readonly ITotalTicketsBookedService _totalTicketsBookedService;

        public TotalTicketsBookedController(ITotalTicketsBookedService totalTicketsBookedService)
        {
            _totalTicketsBookedService = totalTicketsBookedService;
        }

        // GET: api/TotalTicketsBooked/{eventId}
        [HttpGet("{eventId}")]
        public async Task<ActionResult<int>> GetTotalTicketsBooked(Guid eventId)
        {
            try
            {
                var totalTicketsBooked = await _totalTicketsBookedService.GetTotalTicketsBookedAsync(eventId);
                return Ok(totalTicketsBooked);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Event not found");
            }
        }
    }
}
