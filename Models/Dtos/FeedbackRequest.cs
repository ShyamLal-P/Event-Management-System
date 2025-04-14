namespace EventManagementSystem.Models.Dtos
{
    public class FeedbackRequest
    {
        public Guid EventId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
