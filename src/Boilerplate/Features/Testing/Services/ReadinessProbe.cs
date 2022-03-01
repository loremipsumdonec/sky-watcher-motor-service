using System.Diagnostics;
using System.Text;

namespace Boilerplate.Features.Testing.Services
{
    public abstract class ReadinessProbe
        : IReadinessProbe
    {
        private readonly int _timeoutInSeconds;

        public ReadinessProbe()
            : this(60)
        {
        }

        public ReadinessProbe(int timeoutInSeconds) 
        {
            _timeoutInSeconds = timeoutInSeconds;
        }

        public async Task WaitAsync()
        {
            int tick = 0;

            while (!await IsReadyAsync())
            {
                await Task.Delay(1000);

                if (tick++ > _timeoutInSeconds)
                {
                    throw new TimeoutException("Start timedout");
                }
            }
        }

        protected abstract Task<bool> IsReadyAsync();
    }
}
