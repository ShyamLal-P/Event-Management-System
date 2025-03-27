using EventManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AverageRatingController : ControllerBase
    {
        private readonly IAverageRatingService _averageRatingService;

        public AverageRatingController(IAverageRatingService averageRatingService)
        {
            _averageRatingService = averageRatingService;
        }

        [Authorize(Roles ="User, Admin")]
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetAverageRating(Guid eventId)
        {
            var averageRating = await _averageRatingService.GetAverageRatingAsync(eventId);
            return Ok(averageRating);
        }
    }
}
