using Autofac;
using Boilerplate.Features.Core.Config;
using Boilerplate.Features.Mapper.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Boilerplate.Features.Mapper
{
    public class MapperModule
        : Autofac.Module
    {
        public MapperModule(
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
            ConfigureQueryDispatcher(builder);
        }

        private void ConfigureQueryDispatcher(ContainerBuilder builder)
        {
            builder.RegisterFromAs<IModelBuilderRegistry>(
                    "model.builder.registry",
                    Configuration
            ).SingleInstance();

            foreach (Type handlerType in GetTypes<IModelBuilder>())
            {
                builder.RegisterType(handlerType);
            }
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
