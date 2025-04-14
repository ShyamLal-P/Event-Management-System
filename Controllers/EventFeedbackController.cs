using EventManagementSystem.Interface;
using EventManagementSystem.Models;
using EventManagementSystem.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> PostFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            if (feedbackRequest.Rating < 1 || feedbackRequest.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            var hasBookedTicket = await _feedbackService.UserHasBookedTicketAsync(feedbackRequest.EventId, feedbackRequest.UserId);
            if (!hasBookedTicket)
            {
                return BadRequest("User is not registered for the event or does not have a booked ticket.");
            }

            var isEventStarted = await _feedbackService.IsEventStartedAsync(feedbackRequest.EventId);
            if (!isEventStarted)
            {
                var timeUntilEventStarts = await _feedbackService.GetTimeUntilEventStartsAsync(feedbackRequest.EventId);
                return BadRequest($"Feedback will be enabled after the event starts. Time remaining: {timeUntilEventStarts}");
            }

            try
            {
                await _feedbackService.SubmitFeedbackAsync(feedbackRequest.EventId, feedbackRequest.UserId, feedbackRequest.Rating, feedbackRequest.Comments);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Feedback submitted successfully.");
        }
    }
}

