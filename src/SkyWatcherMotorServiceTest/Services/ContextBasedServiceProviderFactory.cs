using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SkyWatcherMotorServiceTest.Services
{
    public class ContextBasedServiceProviderFactory
        : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly IServiceProviderFactory<ContainerBuilder> _decorated;

        public ContextBasedServiceProviderFactory(IServiceProviderFactory<ContainerBuilder> decorated)
        {
            _decorated = decorated;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            return _decorated.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            return new ContextBasedServiceProvider(
                _decorated.CreateServiceProvider(containerBuilder)
            );
        }
    }
}
