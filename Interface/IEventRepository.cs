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
        void SetTotalTickets(Event eventEntity, int totalTickets);
    }
}
