using EventManagementSystem.Interface;
using EventManagementWithAuthentication.Models;

namespace EventManagementSystem.Services
{
    public class TicketBookingService : ITicketBookingService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITicketRepository _ticketRepository;

        public TicketBookingService(IEventRepository eventRepository, ITicketRepository ticketRepository)
        {
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> BookTicketsAsync(Guid userId, Guid eventId, int numberOfTickets)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null || eventItem.NoOfTickets < numberOfTickets || eventItem.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                return false; // Event not found, not enough tickets, or event date is in the past
            }

            var existingTickets = await GetTicketsByUserAndEventAsync(userId, eventId);
            if (existingTickets.Any())
            {
                return false; // User has already booked tickets for this event
            }

            eventItem.NoOfTickets -= numberOfTickets; // Update the number of available tickets
            await _eventRepository.UpdateEventAsync(eventItem);

            for (int i = 0; i < numberOfTickets; i++)
            {
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                    UserId = userId.ToString(),
                    BookingDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = "Booked"
                };
                await _ticketRepository.AddTicketAsync(ticket);
            }

            return true;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserAndEventAsync(Guid userId, Guid eventId)
        {
            return await _ticketRepository.GetTicketsByUserAndEventAsync(userId, eventId);
        }

        public async Task<double> CalculateTotalFareAsync(Guid eventId, int numberOfTickets)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                throw new InvalidOperationException("Event not found.");
            }

            return eventItem.EventPrice * numberOfTickets;
        }
    }
}
