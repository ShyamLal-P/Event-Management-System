using EventManagementSystem.Interface;
using EventManagementSystem.Models.Dtos;
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
        [HttpPost("BookTickets")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> BookTickets([FromBody] BookTicketsRequest request)
        {
            try
            {
                var result = await _ticketBookingService.BookTicketsAsync(request.UserId, request.EventId, request.NumberOfTickets);
                if (!result)
                {
                    return BadRequest("Unable to book tickets. Either the event does not exist, not enough tickets are available, or other business rules were violated.");
                }

                var totalFare = await _ticketBookingService.CalculateTotalFareAsync(request.EventId, request.NumberOfTickets);
                return Ok(new { message = "Tickets booked successfully.", totalFare });
            }
            catch (Exception ex)
            {
                // Log the exception (add a logger if needed)
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
