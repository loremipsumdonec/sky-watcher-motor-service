using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

namespace Boilerplate.Features.Core.Config
{
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterFromAs<T>(
            this ContainerBuilder builder,
            string path,
            IConfiguration config)

        {
            string typeAsString = config.GetSection($"{path}:type").Value;
            Type type = Type.GetType(typeAsString);

            var registrationBuilder = builder.RegisterType(type);

            foreach (var parameter in config.GetSection($"{path}:parameters").GetChildren())
            {
                registrationBuilder.WithParameter(new NamedParameter(parameter.Key, parameter.Value));
            }

            foreach (var decorator in config.GetSection($"{path}:decorators").GetChildren())
            {
                Type decoratorType = Type.GetType(decorator.Value);
                builder.RegisterDecorator(decoratorType, typeof(T));
            }

            return registrationBuilder.As<T>();
        }

        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterFromAs<T>(
            this ContainerBuilder builder,
            string path,
            string name,
            IConfiguration config)
        {
            string typeAsString = config.GetSection($"{path}:type").Value;
            Type type = Type.GetType(typeAsString);

            var registrationBuilder = builder.RegisterType(type);

            foreach (var parameter in config.GetSection($"{path}:{name}:parameters").GetChildren())
            {
                registrationBuilder.WithParameter(new NamedParameter(parameter.Key, parameter.Value));
            }

            foreach (var decorator in config.GetSection($"{path}:decorators").GetChildren())
            {
                Type decoratorType = Type.GetType(decorator.Value);
                builder.RegisterDecorator(decoratorType, typeof(T));
            }

            return registrationBuilder.Keyed<T>(name);
        }
    }
}
