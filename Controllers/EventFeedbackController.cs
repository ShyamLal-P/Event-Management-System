using EventManagementSystem.Interface;
using EventManagementWithAuthentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventFeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public EventFeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> PostFeedback(Guid eventId, string userId, int rating, string comments)
        {
            if (rating < 1 || rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            var hasBookedTicket = await _feedbackService.UserHasBookedTicketAsync(eventId, userId);
            if (!hasBookedTicket)
            {
                return BadRequest("User is not registered for the event or does not have a booked ticket.");
            }

            var isEventStarted = await _feedbackService.IsEventStartedAsync(eventId);
            if (!isEventStarted)
            {
                var timeUntilEventStarts = await _feedbackService.GetTimeUntilEventStartsAsync(eventId);
                return BadRequest($"Feedback will be enabled after the event starts. Time remaining: {timeUntilEventStarts}");
            }

            await _feedbackService.SubmitFeedbackAsync(eventId, userId, rating, comments);

            return Ok("Feedback submitted successfully.");
        }
    }
}
