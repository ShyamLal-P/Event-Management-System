using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models.Dtos
{
    public class CancelTicketsRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of tickets must be at least 1.")]
        public int NumberOfTickets { get; set; }
    }
}
