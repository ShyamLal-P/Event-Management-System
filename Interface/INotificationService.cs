namespace EventManagementSystem.Interface
{
    public interface INotificationService
    {
        Task<string> GetEventNotificationAsync(Guid eventId, string userId);
    }
}
