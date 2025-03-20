namespace EventManagementSystem.Interface
{
    public interface ITicketCancellationService
    {
        Task<(bool Success, string Message)> CancelTicketsAsync(Guid userId, Guid eventId, int numberOfTickets);
        Task<double> CalculateRefundAmountAsync(Guid eventId, int numberOfTickets);
    }
}
