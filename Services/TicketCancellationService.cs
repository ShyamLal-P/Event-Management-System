﻿using EventManagementSystem.Interface;

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

        public async Task<(bool Success, string Message)> CancelTicketsAsync(Guid userId, Guid eventId, int numberOfTickets)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return (false, "Event not found.");
            }

            var currentDateTime = DateTime.Now;
            var eventDateTime = eventItem.Date.ToDateTime(TimeOnly.MinValue).Add(eventItem.Time);

            // Check if the event has already finished
            if (eventDateTime <= currentDateTime)
            {
                return (false, "This event has already finished, so ticket cancellation is not possible.");
            }

            // Check if the event is scheduled to start within the next 24 hours
            var timeUntilEvent = eventDateTime - currentDateTime;
            if (timeUntilEvent <= TimeSpan.FromHours(24))
            {
                return (false, $"The event is going to start in {timeUntilEvent.Hours} hours, so ticket cancellation is not possible.");
            }

            var userTickets = await _ticketRepository.GetTicketsByUserAndEventAsync(userId, eventId);
            var ticketsToCancel = userTickets.Where(t => t.Status == "Booked").Take(numberOfTickets).ToList();

            if (ticketsToCancel.Count < numberOfTickets)
            {
                return (false, "Not enough tickets to cancel.");
            }

            foreach (var ticket in ticketsToCancel)
            {
                ticket.Status = "Cancelled";
                await _ticketRepository.UpdateTicketAsync(ticket);
            }

            eventItem.AvailableTickets += numberOfTickets; // Update the number of available tickets
            await _eventRepository.UpdateEventAsync(eventItem);

            return (true, "Tickets cancelled successfully.");
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
