
using SkyWatcherMotorService.Features.SkyWatcher.Models;

namespace SkyWatcherMotorService.Features.SkyWatcher.Services
{
    public interface ISkyWatcherManager
    {
        Mount GetMount(string mountId);

        IEnumerable<Mount> GetMounts();

        Mount CreateMount(string mountId);

        Motor GetMotor(string motorId);

        Motor CreateMotor(int channel, Mount mount);

        ISkyWatcherMotorClient CreateClient(Motor motor);

        ISkyWatcherMotorClient CreateClient(string motorId);
    }
}
