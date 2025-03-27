using EventManagementSystem.Models;

namespace EventManagementSystem.Interface
{
    public interface ITicketBookingService
    {
        Task<bool> BookTicketsAsync(Guid userId, Guid eventId, int numberOfTickets);
        Task<IEnumerable<Ticket>> GetTicketsByUserAndEventAsync(Guid userId, Guid eventId);
        Task<double> CalculateTotalFareAsync(Guid eventId, int numberOfTickets); // Add this method
    }
}