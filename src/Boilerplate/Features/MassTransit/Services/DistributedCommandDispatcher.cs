using Autofac;
using Boilerplate.Features.Core.Commands;
using MassTransit;
using System.Reflection;

namespace Boilerplate.Features.MassTransit.Services
{
    public class DistributedCommandDispatcher
        : IDistributedCommandDispatcher
    {   
        private readonly ILifetimeScope _scope;
        private readonly ICommandRegistry _registry;

        public DistributedCommandDispatcher(ILifetimeScope scope, ICommandRegistry registry)
        {
            _scope = scope;
            _registry = registry;
        }

        public bool IsDistributed(ICommand command)
        {
            return _registry.GetHandler(command) == null;
        }

        public async Task<bool> DispatchAsync(ICommand command)
        {
            command.CommandResult = await DistributedDispatchAsync(command);
            return command.CommandResult.Status;
        }

        private async Task<CommandResult> DistributedDispatchAsync(ICommand command)
        {
            var genericRequestClientType = typeof(IRequestClient<>).MakeGenericType(command.GetType());

            var client = _scope.Resolve(genericRequestClientType);
            var method = GetResponseMethod(genericRequestClientType);

            var task = (Task)method
                .MakeGenericMethod(typeof(CommandResult))
                .Invoke(client, new object[] { command, default(CancellationToken), default(RequestTimeout) });

            await task.ConfigureAwait(false);
            var response = (Response)task.GetType().GetProperty("Result").GetValue(task);

            return (CommandResult)response.Message;
        }

        private static MethodInfo GetResponseMethod(Type genericRequestClientType)
        {
            var method = genericRequestClientType.GetMethods()
                .FirstOrDefault(m =>
                {
                    if (m.Name != "GetResponse")
                    {
                        return false;
                    }

                    var parameters = m.GetParameters();

                    if (parameters.Length > 2
                        && parameters[0].Name == "message"
                        && parameters[1].Name == "cancellationToken"
                        && m.GetGenericArguments().Length == 1)
                    {
                        return true;
                    }

                    return false;
                });

            if (method == null)
            {
                throw new InvalidOperationException("Could not find method GetResponse method on IRequestClient<> with reflection, check MassTransit for changes");
            }

            return method;
        }
    }
}
