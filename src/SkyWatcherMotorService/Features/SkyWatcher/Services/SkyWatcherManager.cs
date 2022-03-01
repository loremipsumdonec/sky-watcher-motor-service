using SkyWatcherMotorService.Features.SkyWatcher.Models;

namespace SkyWatcherMotorService.Features.SkyWatcher.Services
{
    public class SkyWatcherManager
        : ISkyWatcherManager
    {
        private readonly object _door = new();
        private readonly List<Mount> _mounts = new List<Mount>();
        
        public Mount GetMount(string mountId) 
        {
            return _mounts.Find(m=> mountId.StartsWith(m.MountId));
        }

        public IEnumerable<Mount> GetMounts()
        {
            return _mounts.ToList();
        }

        public Mount CreateMount(string mountId)
        {
            var mount = new Mount(mountId);
            _mounts.Add(mount);

            return mount;
        }

        public Motor GetMotor(string motorId) 
        {
            var mount = GetMount(motorId);
            return mount.Motors.FirstOrDefault(m=> m.MotorId == motorId);
        }

        public Motor CreateMotor(int channel, Mount mount)
        {
            var motor = new Motor($"{mount.MountId}/{channel}")
            {
                BreakSteps = 3500
            };

            mount.Add(motor);

            return motor;
        }

        public ISkyWatcherMotorClient CreateClient(Motor motor) 
        {
            return CreateClient(motor.MotorId);
        }

        public ISkyWatcherMotorClient CreateClient(string motorId)
        {
            var uri = new Uri(motorId);

            int channel = 3;

            if(uri.LocalPath.Length > 1) 
            {
                channel = int.Parse(uri.LocalPath.Substring(1));
            }

            return new SkyWatcherMotorClient(channel, uri.Host, uri.Port, _door);
        }
    }
}
