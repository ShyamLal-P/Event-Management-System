﻿using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EventManagementSystem.Interface;
using EventManagementSystem.Services;
using System.Security.Claims;
using EventManagementSystem.Models.Dtos;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackRepository feedbackRepository, IFeedbackService feedbackService)
        {
            _feedbackRepository = feedbackRepository;
            _feedbackService = feedbackService;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }

        // GET: api/Feedback/5
        [Authorize(Roles = "User, Admin")]
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

        // GET: api/Feedback/User/{userId}
        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetByEventId")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksByEventId([FromBody] EventIdRequest eventIdRequest)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByEventIdAsync(eventIdRequest.EventId);
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound();
            }
            return Ok(feedbacks);
        }

        // POST: api/Feedback
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> PostFeedback([FromBody] Feedback feedback)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure feedback is valid and has all necessary information
            if (feedback == null || string.IsNullOrEmpty(feedback.Comments) || feedback.Rating < 1 || feedback.Rating > 5)
            {
                return BadRequest("Invalid feedback data.");
            }

            // Save feedback through the service layer
            var result = await _feedbackService.PostFeedbackAsync(userId, feedback.EventId, feedback.Rating, feedback.Comments);

            if (result == "Feedback posted successfully.")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // PUT: api/Feedback/5
        [Authorize(Roles = "User, Admin")]
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
        [Authorize(Roles = "User, Admin")]
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
