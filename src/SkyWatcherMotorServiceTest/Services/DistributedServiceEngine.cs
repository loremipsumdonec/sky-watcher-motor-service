using Boilerplate.Features.Testing.Services;
using System.Threading.Tasks;

namespace SkyWatcherMotorServiceTest.Services
{
    public abstract class DistributedServiceEngine
    {
        private readonly IReadinessProbe _readinessProbe;

        public DistributedServiceEngine(IReadinessProbe readinessProbe)
        {
            _readinessProbe = readinessProbe;
        }

        public virtual Task StartAsync() 
        {
            return _readinessProbe.WaitAsync();
        }

        public abstract Task StopAsync();
    }
}
