using EventManagementSystem.Data;
using EventManagementWithAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EventManagementSystem.Interface;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        // GET: api/Feedback
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(Guid id)
        {
            var feedbackItem = await _feedbackRepository.GetFeedbackByIdAsync(id);
            if (feedbackItem == null)
            {
                return NotFound();
            }
            return Ok(feedbackItem);
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedbackItem)
        {
            await _feedbackRepository.AddFeedbackAsync(feedbackItem);
            return CreatedAtAction(nameof(GetFeedback), new { id = feedbackItem.Id }, feedbackItem);
        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(Guid id, [FromBody] Feedback feedbackItem)
        {
            if (id != feedbackItem.Id)
            {
                return BadRequest("Feedback ID mismatch.");
            }

            if (!_feedbackRepository.FeedbackExists(id))
            {
                return NotFound();
            }

            await _feedbackRepository.UpdateFeedbackAsync(feedbackItem);
            return NoContent();
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            if (!_feedbackRepository.FeedbackExists(id))
            {
                return NotFound();
            }

            await _feedbackRepository.DeleteFeedbackAsync(id);
            return NoContent();
        }
    }
}