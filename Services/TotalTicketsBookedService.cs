using EventManagementSystem.Interface;

namespace EventManagementSystem.Services
{
    public class TotalTicketsBookedService : ITotalTicketsBookedService
    {
        private readonly IEventRepository _eventRepository;

        public TotalTicketsBookedService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<int> GetTotalTicketsBookedAsync(Guid eventId)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                throw new KeyNotFoundException("Event not found");
            }
            return eventItem.TotalTickets - eventItem.AvailableTickets;
        }
    }
}
