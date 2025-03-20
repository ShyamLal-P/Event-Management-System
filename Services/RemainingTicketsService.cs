using EventManagementSystem.Interface;

namespace EventManagementSystem.Services
{
    public class RemainingTicketsService : IRemainingTicketsService
    {
        private readonly IEventRepository _eventRepository;

        public RemainingTicketsService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<int> GetRemainingTicketsAsync(Guid eventId)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                throw new KeyNotFoundException("Event not found");
            }
            return eventItem.NoOfTickets;
        }
    }
}
