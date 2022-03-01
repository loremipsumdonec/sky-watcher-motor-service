using Autofac;
using Autofac.Features.AttributeFilters;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Config;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Core.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Boilerplate.Features.Core
{
    public class CoreModule
        : Autofac.Module
    {
        public CoreModule(
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
            ConfigureAssemblies(builder);
            ConfigureQueryDispatcher(builder);
            ConfigureCommandDispatcher(builder);
            ConfigureHeartbeatDispatcher(builder);
            ConfigureModelService(builder);
            ConfigureConnectionStrings(builder);
            //ConfigureModelTransformRegistry(engine);
        }

        private void ConfigureConnectionStrings(ContainerBuilder builder)
        {
            foreach (var item in Configuration.GetSection("ConnectionStrings")?.GetChildren())
            {
                builder.RegisterInstance(item.Value).Keyed<string>($"{item.Key}ConnectionString");
            }
        }

        private void ConfigureAssemblies(ContainerBuilder builder)
        {
            builder.Register<IAssemblies>(
                _ => new Assemblies(Assemblies)
            ).SingleInstance();
        }

        private void ConfigureQueryDispatcher(ContainerBuilder builder)
        {
            builder.RegisterFromAs<IQueryRegistry>(
                    "query.registry",
                    Configuration
            ).SingleInstance();

            builder.RegisterFromAs<IQueryDispatcher>(
                    "query.dispatcher",
                    Configuration
            ).InstancePerLifetimeScope();

            foreach (Type handlerType in GetTypes<IQueryHandler>())
            {
                builder.RegisterType(handlerType).WithAttributeFiltering();
            }
        }

        private void ConfigureCommandDispatcher(ContainerBuilder builder)
        {
            builder.RegisterFromAs<ICommandRegistry>(
                    "command.registry",
                    Configuration
            ).SingleInstance();

            builder.RegisterFromAs<ICommandDispatcher>(
                    "command.dispatcher",
                    Configuration
            );

            foreach (Type handlerType in GetTypes<ICommandHandler>())
            {
                builder.RegisterType(handlerType).WithAttributeFiltering();
            }
        }

        private void ConfigureHeartbeatDispatcher(ContainerBuilder builder)
        {
            builder.RegisterFromAs<IHeartbeatDispatcher>(
                    "heartbeat.dispatcher",
                    Configuration
            ).InstancePerLifetimeScope();
        }

        private void ConfigureModelService(ContainerBuilder builder)
        {
            builder.RegisterFromAs<IModelRegistry>(
                    "model.registry",
                    Configuration
            ).SingleInstance();

            builder.RegisterFromAs<IModelService>(
                    "model.service",
                    Configuration
            ).InstancePerLifetimeScope();
        }

        private IEnumerable<Type> GetTypes<T>()
        {
            foreach (Assembly assembly in Assemblies)
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
