namespace EventManagementSystem.Interface
{
    public interface IRemainingTicketsService
    {
        Task<int> GetRemainingTicketsAsync(Guid eventId);
    }
}