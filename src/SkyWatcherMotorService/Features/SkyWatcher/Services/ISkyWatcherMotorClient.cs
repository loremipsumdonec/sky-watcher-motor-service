namespace SkyWatcherMotorService.Features.SkyWatcher.Services
{
    public interface ISkyWatcherMotorClient
        : IDisposable
    {
        ValueTask<(long, long, long, long)> SendAndReciveSectionsAsync(string header);

        ValueTask<long> SendAndRecieveAsync(string header);

        Task SendAsync(string header, long data, int length = 6);
    }
}
