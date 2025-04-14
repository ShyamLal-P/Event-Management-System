using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using EventManagementSystem.Models.Dtos;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/Events
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return Ok(events);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("user/{organizerId}")]
        public async Task<IActionResult> GetEventsByOrganizerId(string organizerId)
        {
            var events = await _eventRepository.GetEventsByOrganizerIdAsync(organizerId);
            return Ok(events);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(Guid id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return Ok(eventItem);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<Event>>> GetUpcomingEvents()
        {
            var events = await _eventRepository.GetUpcomingEventsAsync();
            return Ok(events);
        }



        [Authorize(Roles = "User, Admin")]
        [HttpGet("top-events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetTopEventsByBookingRatio()
        {
            var events = await _eventRepository.GetTopEventsByBookingRatioAsync();
            return Ok(events);
        }


        // POST: api/Events
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent([FromBody] Event eventItem)
        {
            eventItem.SetTotalTickets(eventItem.TotalTickets); // Ensure NoOfTickets is set
            await _eventRepository.AddEventAsync(eventItem);
            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);
        }

        // PUT: api/Events/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] Event eventItem)
        {
            if (id != eventItem.Id)
            {
                return BadRequest("Event ID mismatch.");
            }

            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            // Update the properties of the existing event with the new values
            existingEvent.Name = eventItem.Name;
            existingEvent.Category = eventItem.Category;
            existingEvent.Location = eventItem.Location;
            existingEvent.Date = eventItem.Date;
            existingEvent.Time = eventItem.Time;
            existingEvent.TotalTickets = eventItem.TotalTickets;
            existingEvent.EventPrice = eventItem.EventPrice;
            existingEvent.OrganizerId = eventItem.OrganizerId;

            // Automatically update NoOfTickets based on TotalTickets
            existingEvent.SetTotalTickets(eventItem.TotalTickets);

            await _eventRepository.UpdateEventAsync(existingEvent);
            return NoContent();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("past-events-without-feedback")]
        public async Task<ActionResult<IEnumerable<Event>>> GetPastEventsWithoutFeedback([FromBody] PastEventFeedbackCheckRequestDto request)
        {
            if (string.IsNullOrEmpty(request.UserId))
                return BadRequest("UserId is required.");

            var events = await _eventRepository.GetPastEventsWithoutFeedbackAsync(request.UserId);
            return Ok(events);
        }

        /// <summary>
        /// This delete when 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Events/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            if (!_eventRepository.EventExists(id))
            {
                return NotFound();
            }

            await _eventRepository.DeleteEventAsync(id);
            return NoContent();
        }
    }
}