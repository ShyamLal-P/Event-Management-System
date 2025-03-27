namespace EventManagementSystem.Interface
{
    public interface IAverageRatingService
    {
        Task<double> GetAverageRatingAsync(Guid eventId);
    }
}
