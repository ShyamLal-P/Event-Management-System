using EventManagementSystem.Models;

namespace EventManagementSystem.Interface
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(Guid id);
        Task AddEventAsync(Event eventItem);
        Task UpdateEventAsync(Event eventItem);
        Task DeleteEventAsync(Guid id);
        bool EventExists(Guid id);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> GetTopEventsByBookingRatioAsync();
        void SetTotalTickets(Event eventEntity, int totalTickets);
        Task<IEnumerable<Event>> GetEventsByIdsAsync(List<Guid> eventIds);
        Task<IEnumerable<Event>> GetPastEventsWithoutFeedbackAsync(string userId);
        Task<IEnumerable<Event>> GetEventsByOrganizerIdAsync(string organizerId);
    }
}
