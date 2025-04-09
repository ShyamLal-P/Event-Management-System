namespace EventManagementSystem.Models.Dtos
{
    public class CancelTicketsRequest
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int NumberOfTickets { get; set; }

    }
}
