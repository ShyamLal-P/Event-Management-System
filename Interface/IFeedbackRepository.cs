using EventManagementSystem.Models;

namespace EventManagementSystem.Interface
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(Guid id);
        Task AddFeedbackAsync(Feedback feedbackItem);
        Task<IEnumerable<Feedback>> GetFeedbacksByEventIdAsync(Guid eventId);
        Task UpdateFeedbackAsync(Feedback feedbackItem);
        Task DeleteFeedbackAsync(Guid id);
        bool FeedbackExists(Guid id);
    }
}
