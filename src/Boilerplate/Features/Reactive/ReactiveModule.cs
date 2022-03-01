using Autofac;
using Boilerplate.Features.Core.Config;
using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Boilerplate.Features.Reactive.Reactive
{
    public class ReactiveModule
        : Autofac.Module
    {
        public ReactiveModule(
            IConfiguration configuration,
            List<Assembly> assemblies)
        {
            Configuration = configuration;
            Assemblies = assemblies;
        }

        public IConfiguration Configuration { get; }

        public List<Assembly> Assemblies { get; }

        protected override void Load(ContainerBuilder builder)
        {
            ConfigureEventDispatcher(builder);

            builder.RegisterBuildCallback(scope =>
                scope.Resolve<IEventHub>().Open()
            );
        }

        private void ConfigureEventDispatcher(ContainerBuilder builder)
        {
            builder.RegisterFromAs<IEventHandlerRegistry>(
                    "event.handler.registry",
                    Configuration
            ).SingleInstance();

            builder.RegisterFromAs<IEventHub>(
                    "event.hub",
                    Configuration
            ).SingleInstance();

            builder.RegisterFromAs<IEventDispatcher>(
                "event.dispatcher",
                Configuration
            ).InstancePerLifetimeScope();

            foreach (Type handlerType in GetTypes<IEventHandler>(Assemblies))
            {
                builder.RegisterType(handlerType);
            }
        }

        private IEnumerable<Type> GetTypes<T>(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                var types = assembly.GetExportedTypes()
                    .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }
    }
}
