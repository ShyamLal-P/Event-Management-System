namespace EventManagementSystem.Interface
{
    public interface IFeedbackService
    {
        Task<bool> UserHasBookedTicketAsync(Guid eventId, string userId);
        Task<bool> IsEventStartedAsync(Guid eventId);
        Task<string> GetTimeUntilEventStartsAsync(Guid eventId);
        Task SubmitFeedbackAsync(Guid eventId, string userId, Guid ticketId, int rating, string comments);
        Task<bool> FeedbackExistsForTicketAsync(Guid ticketId); // New method
    }
}
