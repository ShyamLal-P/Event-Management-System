using EventManagementSystem.Interface;

namespace EventManagementSystem.Services
{
    public class TicketCancellationService : ITicketCancellationService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITicketRepository _ticketRepository;

        public TicketCancellationService(IEventRepository eventRepository, ITicketRepository ticketRepository)
        {
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> CancelTicketsAsync(Guid userId, Guid eventId, int numberOfTickets)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null || eventItem.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                return false; // Event not found or event date is in the past
            }

            var userTickets = await _ticketRepository.GetTicketsByUserAndEventAsync(userId, eventId);
            var ticketsToCancel = userTickets.Where(t => t.Status == "Booked").Take(numberOfTickets).ToList();

            if (ticketsToCancel.Count < numberOfTickets)
            {
                return false; // Not enough tickets to cancel
            }

            foreach (var ticket in ticketsToCancel)
            {
                ticket.Status = "Cancelled";
                await _ticketRepository.UpdateTicketAsync(ticket);
            }

            eventItem.NoOfTickets += numberOfTickets; // Update the number of available tickets
            await _eventRepository.UpdateEventAsync(eventItem);

            return true;
        }

        public async Task<double> CalculateRefundAmountAsync(Guid eventId, int numberOfTickets)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                throw new InvalidOperationException("Event not found.");
            }

            double totalFare = eventItem.EventPrice * numberOfTickets;
            double refundAmount = totalFare * 0.70; // Refund 70% of the total fare
            return refundAmount;
        }
    }
}
