using EventManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ITicketBookingService _ticketBookingService;

        public BookingController(ITicketBookingService ticketBookingService)
        {
            _ticketBookingService = ticketBookingService;
        }

        // POST: api/Booking/BookTickets
        [Authorize(Roles ="Admin, User")]
        [HttpPost("BookTickets")]
        public async Task<IActionResult> BookTickets(Guid userId, Guid eventId, int numberOfTickets)
        {
            var result = await _ticketBookingService.BookTicketsAsync(userId, eventId, numberOfTickets);
            if (!result)
            {
                return BadRequest("Unable to book tickets. Either the event does not exist, there are not enough tickets available, or other business rules were violated.");
            }

            var totalFare = await _ticketBookingService.CalculateTotalFareAsync(eventId, numberOfTickets);
            return Ok(new { message = "Tickets booked successfully.", totalFare });
        }
    }
}
