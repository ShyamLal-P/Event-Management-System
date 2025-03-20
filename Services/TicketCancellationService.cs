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
            if (eventItem == null)
            {
                return false; // Event not found
            }

            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            // Check if the event has already finished
            if (eventItem.Date < currentDate || (eventItem.Date == currentDate && eventItem.Time < currentTime))
            {
                return false; // Event has already finished
            }

            // Check if the event is scheduled to start within the next 24 hours
            var eventDateTime = eventItem.Date.ToDateTime(eventItem.Time);
            if (eventDateTime <= DateTime.Now.AddHours(24))
            {
                return false; // Event is scheduled to start within the next 24 hours
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
