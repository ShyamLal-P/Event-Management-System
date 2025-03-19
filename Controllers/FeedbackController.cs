using EventManagementSystem.Data;
using EventManagementWithAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly EventManagementSystemDbContext _context;

        public FeedbackController(EventManagementSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Feedback
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(Guid id)
        {
            var feedbackItem = await _context.Feedbacks.FindAsync(id);

            if (feedbackItem == null)
            {
            }

            return feedbackItem;
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedbackItem)
        {
            feedbackItem.Id = Guid.NewGuid();
            _context.Feedbacks.Add(feedbackItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedback), new { id = feedbackItem.Id }, feedbackItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(Guid id, [FromBody] Feedback feedbackItem)
        {
            if (id != feedbackItem.Id)
            {
                return BadRequest("Feedback ID mismatch.");
            }

            var existingFeedback = await _context.Feedbacks.FindAsync(id);
            if (existingFeedback == null)
            {
                return NotFound();
            }

            existingFeedback.Rating = feedbackItem.Rating;
            existingFeedback.Comments = feedbackItem.Comments;
            existingFeedback.SubmittedTimestamp = feedbackItem.SubmittedTimestamp;

            _context.Entry(existingFeedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool FeedbackExists(Guid id)
        {
            return _context.Feedbacks.Any(f => f.Id == id);
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var feedbackItem = await _context.Feedbacks.FindAsync(id);
            if (feedbackItem == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedbackItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}