using Autofac;
using Boilerplate.Features.Core.Config;
using Boilerplate.Features.MassTransit.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Boilerplate.Features.MassTransit
{
    public class MassTransitModule
        : Autofac.Module
    {
        public MassTransitModule(
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
            builder.RegisterFromAs<IDistributedQueryDispatcher>(
                    "distributed.query.dispatcher",
                    Configuration
            ).InstancePerLifetimeScope();

            builder.RegisterFromAs<IDistributedCommandDispatcher>(
                    "distributed.command.dispatcher",
                    Configuration
            ).InstancePerLifetimeScope();
        }
    }
}
