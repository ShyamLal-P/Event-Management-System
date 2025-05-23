﻿namespace EventManagementSystem.Interface
{
    public interface IFeedbackService
    {
        Task<bool> UserHasBookedTicketAsync(Guid eventId, string userId);
        Task<bool> IsEventStartedAsync(Guid eventId);
        Task<string> PostFeedbackAsync(string userId, Guid eventId, int rating, string comments);

        Task<string> GetTimeUntilEventStartsAsync(Guid eventId);
        Task SubmitFeedbackAsync(Guid eventId, string userId, int rating, string comments);
    }
}
