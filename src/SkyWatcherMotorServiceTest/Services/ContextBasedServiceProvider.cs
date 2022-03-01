using System;

namespace SkyWatcherMotorServiceTest.Services
{
    public class ContextBasedServiceProvider
        : IServiceProvider
    {
        private readonly IServiceProvider _decorated;

        public ContextBasedServiceProvider(IServiceProvider decorated)
        {
            _decorated = decorated;
        }

        public object? GetService(Type serviceType)
        {
            return _decorated.GetService(serviceType);
        }
    }
}
