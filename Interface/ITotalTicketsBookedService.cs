namespace EventManagementSystem.Interface
{
    public interface ITotalTicketsBookedService
    {
        Task<int> GetTotalTicketsBookedAsync(Guid eventId);
    }
}
