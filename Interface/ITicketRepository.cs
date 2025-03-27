using EventManagementSystem.Models;

namespace EventManagementSystem.Interface
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket> GetTicketByIdAsync(Guid id);
        Task AddTicketAsync(Ticket ticketItem);
        Task UpdateTicketAsync(Ticket ticketItem);
        Task DeleteTicketAsync(Guid id);
        bool TicketExists(Guid id);
        Task<IEnumerable<Ticket>> GetTicketsByUserAndEventAsync(Guid userId, Guid eventId);
    }
}
